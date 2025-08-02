using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using ExpenseTracker.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.ExpenseReports
{
    public class ExpenseReportRepository
        : EfCoreRepository<ExpenseTrackerDbContext, ExpenseReport, Guid>,
            IExpenseReportRepository
    {
        public ExpenseReportRepository(IDbContextProvider<ExpenseTrackerDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public async Task<int> GetPendingReportsCountAsync(Guid ownerId)
        {
            var dbContext = await GetDbContextAsync();

            return await dbContext.ExpenseReports
                .Where(x => x.OwnerId == ownerId && x.Status == "Pending")
                .CountAsync();
        }

        public async Task InsertAsync(ExpenseReport report, bool autoSave)
        {
            var dbContext = await GetDbContextAsync();
            await dbContext.ExpenseReports.AddAsync(report);

            if (autoSave)
            {
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<List<ExpenseReport>> GetListAsync()
        {
            var dbContext = await GetDbContextAsync();
            return await dbContext.ExpenseReports.ToListAsync();
        }
    }
}