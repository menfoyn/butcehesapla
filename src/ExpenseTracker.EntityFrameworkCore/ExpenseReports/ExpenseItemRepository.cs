using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.EntityFrameworkCore;

using Volo.Abp.EntityFrameworkCore;



namespace ExpenseTracker.ExpenseReports;

public class ExpenseItemRepository
    : EfCoreRepository<ExpenseTrackerDbContext, ExpenseItem, Guid>, IExpenseItemRepository
{
    public ExpenseItemRepository(IDbContextProvider<ExpenseTrackerDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
        
    }

    public async Task<List<ExpenseItem>> GetItemsByReportIdAsync(Guid reportId)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.ExpenseItems
            .Where(x => x.ExpenseReportId == reportId)
            .ToListAsync();
    }
}
