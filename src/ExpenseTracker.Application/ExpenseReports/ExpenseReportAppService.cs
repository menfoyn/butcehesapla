using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.ExpenseReports;
using ExpenseTracker.ExpenseReports.Services;
using Microsoft.Extensions.Logging;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;
using Volo.Abp.MultiTenancy;

namespace ExpenseTracker.ExpenseReports.Services
{
    public class ExpenseReportAppService : ApplicationService, IExpenseReportAppService
    {
        private readonly IExpenseReportRepository _expenseReportRepository;

        public ExpenseReportAppService(IExpenseReportRepository expenseReportRepository)
        {
            _expenseReportRepository = expenseReportRepository;
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
                    var item = new ExpenseItem
                    {
                        Date = i.Date == default ? DateTime.Now : i.Date,
                        Amount = i.Amount,
                        Description = i.Description,
                        CategoryId = i.CategoryId,
                        Currency = string.IsNullOrWhiteSpace(i.Currency) ? "TRY" : i.Currency,
                        WorkedHours = i.WorkedHours,
                        Name = i.Name,
                        ReceiptImagePath = i.ReceiptImagePath
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
            var expenseReport = await _expenseReportRepository.GetAsync(id);
            return ObjectMapper.Map<ExpenseReport, ExpenseReportDto>(expenseReport);
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