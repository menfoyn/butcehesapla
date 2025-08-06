using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;


namespace ExpenseTracker.ExpenseReports;

public interface IExpenseReportRepository : IRepository<ExpenseReport, Guid>


{
    Task<int> GetPendingReportsCountAsync(Guid ownerId);
    Task<ExpenseReport> GetAsync(Guid ownerId);
}