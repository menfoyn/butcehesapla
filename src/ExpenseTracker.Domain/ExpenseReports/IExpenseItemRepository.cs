using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;


namespace ExpenseTracker.ExpenseReports;

public interface IExpenseItemRepository : IRepository<ExpenseItem, Guid>
{
    Task<List<ExpenseItem>> GetItemsByReportIdAsync(Guid reportId);

}