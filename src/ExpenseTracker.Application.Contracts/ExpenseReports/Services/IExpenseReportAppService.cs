using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;


namespace ExpenseTracker.ExpenseReports.Services;

public interface IExpenseReportAppService : IApplicationService


{
    Task<List<ExpenseReportDto>>GetListAsync();
    Task<ExpenseReportDto> GetAsync(Guid id);
    Task<ExpenseReportDto> CreateAsync(CreateExpenseReportDto input);
    Task DeleteAsync(Guid id);
    
}