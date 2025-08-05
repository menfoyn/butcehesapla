using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Users;
using Volo.Abp.Domain.Entities.Auditing;

namespace ExpenseTracker.ExpenseReports
{
    public class ExpenseReport : IHasCreator<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public decimal SpendingLimit { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ReceiptFilePath { get; set; }

        public Guid? CreatorId { get; set; }

        private List<ExpenseItem> _items = new List<ExpenseItem>();
        public IReadOnlyList<ExpenseItem> Items => _items.AsReadOnly();

        public void AddItem(ExpenseItem item)
        {
            _items.Add(item);
        }
    }

    public interface IHasCreator<T>
    {
    }

    public class ExpenseReportAppService : ApplicationService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IObjectMapper ObjectMapper;
        private readonly ICurrentUser _currentUser;

        public ExpenseReportAppService(
            IReportRepository reportRepository,
            IObjectMapper objectMapper,
            ICurrentUser currentUser)
        {
            _reportRepository = reportRepository;
            ObjectMapper = objectMapper;
            _currentUser = currentUser;
        }

        public async Task<ExpenseReportDto> CreateAsync(CreateExpenseReportDto input)
        {
            var items = new List<ExpenseItem>();

            if (input.Items != null)
            {
                foreach (var itemDto in input.Items)
                {
                    var item = new ExpenseItem
                    {
                        Name = itemDto.Description,
                        Amount = itemDto.Amount,
                        Currency = itemDto.Currency ?? "TRY",
                        ReceiptPath = itemDto.ReceiptPaths?.FirstOrDefault()
                    };

                    items.Add(item);
                }
            }

            var report = new ExpenseReport
            {
                Id = Guid.NewGuid(),
                Title = input.Title,
                ProjectId = input.ProjectId != Guid.Empty ? input.ProjectId : Guid.NewGuid(),
                SpendingLimit = input.SpendingLimit ?? 0,
                CreatedAt = DateTime.Now,
                Status = "Taslak",
                ReceiptFilePath = input.Items?.FirstOrDefault()?.ReceiptPaths?.FirstOrDefault() ?? string.Empty,
                CreatorId = _currentUser.Id,
            };

            foreach (var item in items)
            {
                report.AddItem(item);
            }

            await _reportRepository.InsertAsync(report, autoSave: true);

            return ObjectMapper.Map<ExpenseReport, ExpenseReportDto>(report);
        }

        public async Task<List<ExpenseReportDto>> GetListAsync()
        {
            var reports = await _reportRepository.GetListAsync();

            // SADECE kendi oluşturduğu raporları görsün
            if (!_currentUser.Roles.Contains("Admin"))
            {
                reports = reports.Where(r => r.CreatorId == _currentUser.Id).ToList();
            }

            return reports.Select(report => new ExpenseReportDto
            {
                Id = report.Id,
                Title = report.Title,
                TotalAmount = report.Items.Sum(i => i.Amount),
                Status = report.Status
            }).ToList();
        }
    }

    public interface IReportRepository
    {
        Task InsertAsync(ExpenseReport report, bool autoSave);
        Task<List<ExpenseReport>> GetListAsync();
    }
}