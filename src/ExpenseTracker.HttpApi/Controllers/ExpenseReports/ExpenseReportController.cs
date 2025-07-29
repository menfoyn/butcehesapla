using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using ExpenseTracker.ExpenseReports;
using ExpenseTracker.ExpenseReports.Services;

namespace ExpenseTracker.Controllers.ExpenseReports
{
    [Route("api/expense-reports")]
    public class ExpenseReportController : AbpController
    {
        private readonly IExpenseReportAppService _expenseReportAppService;

        public ExpenseReportController(IExpenseReportAppService expenseReportAppService)
        {
            _expenseReportAppService = expenseReportAppService;
        }

        [HttpGet]
        public Task<List<ExpenseReportDto>> GetListAsync()
        {
            return _expenseReportAppService.GetListAsync();
        }

        [HttpGet("{id}")]
        public Task<ExpenseReportDto> GetAsync(Guid id)
        {
            return _expenseReportAppService.GetAsync(id);
        }

        [HttpPost]
        public Task<ExpenseReportDto> CreateAsync(CreateExpenseReportDto input)
        {
            return _expenseReportAppService.CreateAsync(input);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _expenseReportAppService.DeleteAsync(id);
        }
    }
}