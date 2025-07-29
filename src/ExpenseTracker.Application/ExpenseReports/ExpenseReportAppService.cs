using ExpenseTracker.ExpenseReports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using ExpenseTracker.ExpenseReports;
using ExpenseTracker.ExpenseReports.Services;

namespace ExpenseTracker.ExpenseReports
{
    public class ExpenseReportAppService :ApplicationService,IExpenseReportAppService
      
    {
        private readonly IExpenseReportRepository _expenseReportRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly IObjectMapper _objectMapper;

        public ExpenseReportAppService(
            IExpenseReportRepository expenseReportRepository,
            IRepository<Category, Guid> categoryRepository,
            IObjectMapper objectMapper)
        {
            _expenseReportRepository = expenseReportRepository;
            _categoryRepository = categoryRepository;
            _objectMapper = objectMapper;
        }

        public async Task<List<ExpenseReportDto>> GetListAsync()
        {
            var reports = await _expenseReportRepository.GetListAsync(includeDetails: true);
            return _objectMapper.Map<List<ExpenseReport>, List<ExpenseReportDto>>(reports);
        }

        public async Task<ExpenseReportDto> GetAsync(Guid id)
        {
            var report = await _expenseReportRepository.GetAsync(id);
            return _objectMapper.Map<ExpenseReport, ExpenseReportDto>(report);
        }

        public async Task<ExpenseReportDto> CreateAsync(CreateExpenseReportDto input)
        {
            var report = new ExpenseReport
            {
                OwnerId = input.OwnerId,
                Status = "Pending"
            };

            foreach (var item in input.Items)
            {
                report.AddItem(new ExpenseItem
                {
                    Date = item.Date,
                    Amount = item.Amount,
                    Description = item.Description,
                    CategoryId = item.CategoryId,
                    Currency = item.Currency,
                    ReceiptImagePath = item.ReceiptImagePath
                });
            }

            await _expenseReportRepository.InsertAsync(report, autoSave: true);

            return _objectMapper.Map<ExpenseReport, ExpenseReportDto>(report);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _expenseReportRepository.DeleteAsync(id);
        }
    }

    
}