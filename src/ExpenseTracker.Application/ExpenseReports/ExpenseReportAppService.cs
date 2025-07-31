using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.ObjectMapping;

namespace ExpenseTracker.ExpenseReports
{
    public class ExpenseReport
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ProjectId { get; set; }
        public decimal SpendingLimit { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public string? ReceiptFilePath { get; set; }

        private List<ExpenseItem> _items = new List<ExpenseItem>();
        public IReadOnlyList<ExpenseItem> Items => _items.AsReadOnly();

        public void AddItem(ExpenseItem item)
        {
            _items.Add(item);
        }
    }

    public class ExpenseReportAppService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IObjectMapper ObjectMapper;

        public ExpenseReportAppService(IReportRepository reportRepository, IObjectMapper objectMapper)
        {
            _reportRepository = reportRepository;
            ObjectMapper = objectMapper;
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
                ReceiptFilePath = input.Items?.FirstOrDefault()?.ReceiptPaths?.FirstOrDefault() ?? string.Empty
            };

            foreach (var item in items)
            {
                report.AddItem(item);
            }

            await _reportRepository.InsertAsync(report, autoSave: true);

            return ObjectMapper.Map<ExpenseReport, ExpenseReportDto>(report);
        }
    }

    public interface IReportRepository
    {
        Task InsertAsync(ExpenseReport report, bool autoSave);
    }
}