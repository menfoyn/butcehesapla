using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.ExpenseReports;
using ExpenseTracker.ExpenseReports.Services;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

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
                Status = "Pending"  // İstersen burada varsayılan durum ver
            };  

            await _expenseReportRepository.InsertAsync(expenseReport, autoSave: true);

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
            var reports = await _expenseReportRepository.GetListAsync();
            return reports.Select(r => ObjectMapper.Map<ExpenseReport, ExpenseReportDto>(r)).ToList();
        }
    }
}