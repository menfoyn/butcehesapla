using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.ExpenseReports;
using ExpenseTracker.ExpenseReports.Services;
using ExpenseTracker.Projects;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using Volo.Abp.MultiTenancy;

namespace ExpenseTracker.ExpenseReports.Services
{
    public class ExpenseReportAppService : ApplicationService, IExpenseReportAppService
    {
        private readonly IExpenseReportRepository _expenseReportRepository;

        // ↓↓↓ EKLENEN BAĞIMLILIKLAR ↓↓↓
        private readonly ICategoryRepository _categoryRepository;
        private readonly IRepository<Project, Guid> _projectRepository;

        public ExpenseReportAppService(
            IExpenseReportRepository expenseReportRepository,
            ICategoryRepository categoryRepository,
            IRepository<Project, Guid> projectRepository)
        {
            _expenseReportRepository = expenseReportRepository;
            _categoryRepository = categoryRepository;
            _projectRepository = projectRepository;
        }

        // UI sadece kategori ADI göndermişse ID’yi çöz
        private async Task<Guid> ResolveCategoryIdAsync(string? categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return Guid.Empty;

            var existing = await _categoryRepository.FindByNameAsync(categoryName.Trim());
            if (existing != null)
                return existing.Id;

            // Id'yi ctor üzerinden veriyoruz
            var created = new Category(GuidGenerator.Create())
            {
                Name = categoryName.Trim()
            };
            await _categoryRepository.InsertAsync(created, autoSave: true);
            return created.Id;
        }
        public async Task<ExpenseReportDto> CreateAsync(CreateExpenseReportDto input)
        {
            var expenseReport = new ExpenseReport(
                projectId: input.ProjectId,
                spendingLimit: input.SpendingLimit,
                createdAt: DateTime.Now,
                receiptFilePath: input.ReceiptFilePath,
                creatorId: CurrentUser.Id
            )
            {
                Title = input.Title,
                Status = "Pending"
            };

            // Map items from DTO to entity and compute totals
            if (input.Items != null && input.Items.Count > 0)
            {
                foreach (var i in input.Items)
                {
                    // Eğer CategoryId boş ve Category adı doluysa ID’yi çöz
                    var categoryId = i.CategoryId;
                    if (categoryId == Guid.Empty && !string.IsNullOrWhiteSpace(i.Category))
                        categoryId = await ResolveCategoryIdAsync(i.Category);

                    var item = new ExpenseItem
                    {
                        Date = i.Date == default ? DateTime.Now : i.Date,
                        Amount = i.Amount,
                        Description = i.Description,
                        CategoryId = categoryId,
                        Currency = string.IsNullOrWhiteSpace(i.Currency) ? "TRY" : i.Currency,
                        WorkedHours = i.WorkedHours,
                        Name = i.Name,
                        ReceiptImagePath = i.ReceiptImagePath,
                        ReceiptPaths = i.ReceiptPaths ?? new List<string>()
                    };

                    expenseReport.AddItem(item);
                }

                expenseReport.TotalAmount = expenseReport.Items.Sum(x => x.Amount);
            }

            // Set owner from the current user (ignore DTO OwnerId)
            if (CurrentUser?.Id != null)
            {
                expenseReport.OwnerId = CurrentUser.Id.Value;
            }

            Logger.LogInformation("[ExpenseReport] Create called. CurrentTenant={TenantId}, CurrentUser={UserId}", CurrentTenant?.Id, CurrentUser?.Id);
            Logger.LogInformation("[ExpenseReport] Insert starting. Title={Title}, Items={ItemCount}, OwnerId={OwnerId}", expenseReport.Title, expenseReport.Items.Count, expenseReport.OwnerId);

            await _expenseReportRepository.InsertAsync(expenseReport, autoSave: true);

            // Extra safety for Blazor-Server direct calls – ensure the UoW flushes
            await CurrentUnitOfWork.SaveChangesAsync();

            Logger.LogInformation("[ExpenseReport] Insert finished. NewId={Id}, TotalAmount={Total}", expenseReport.Id, expenseReport.TotalAmount);

            return ObjectMapper.Map<ExpenseReport, ExpenseReportDto>(expenseReport);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _expenseReportRepository.DeleteAsync(id);
        }

        public async Task<ExpenseReportDto> GetAsync(Guid id)
        {
            var entity = await _expenseReportRepository.GetAsync(id);
            var dto = ObjectMapper.Map<ExpenseReport, ExpenseReportDto>(entity);

            // Proje adını doldur
            if (dto != null && dto.ProjectId != Guid.Empty)
            {
                var proj = await _projectRepository.FindAsync(dto.ProjectId);
                if (proj != null) dto.ProjectName = proj.Name;
            }

            // Kategori adlarını doldur
            if (entity?.Items != null && entity.Items.Count > 0 && dto?.Items != null)
            {
                var catIds = entity.Items
                    .Select(i => i.CategoryId)
                    .Where(x => x != Guid.Empty)
                    .Distinct()
                    .ToList();

                var catDict = new Dictionary<Guid, string>();
                if (catIds.Count > 0)
                {
                    var allCats = await _categoryRepository.GetListAsync();
                    foreach (var c in allCats.Where(c => catIds.Contains(c.Id)))
                        catDict[c.Id] = c.Name;
                }

                foreach (var itemDto in dto.Items)
                {
                    if (itemDto.CategoryId != Guid.Empty && catDict.TryGetValue(itemDto.CategoryId, out var name))
                        itemDto.Category = name;
                }
            }

            return dto;
        }

        public async Task<List<ExpenseReportDto>> GetListAsync()
        {
            Logger.LogInformation("[ExpenseReport] GetList called. CurrentTenant={TenantId}, CurrentUser={UserId}", CurrentTenant?.Id, CurrentUser?.Id);
            var reports = await _expenseReportRepository.GetListAsync();
            Logger.LogInformation("[ExpenseReport] GetList returned {Count} rows", reports?.Count ?? -1);
            return reports.Select(r => ObjectMapper.Map<ExpenseReport, ExpenseReportDto>(r)).ToList();
        }
    }
}