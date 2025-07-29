using System;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore;
using ExpenseTracker.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;


namespace ExpenseTracker.ExpenseReports;

public class CategoryRepository
:EfCoreRepository<ExpenseTrackerDbContext,Category, Guid>, ICategoryRepository

{
    public CategoryRepository(IDbContextProvider<ExpenseTrackerDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
        
    }

    public async Task<Category> FindByNameAsync(string name)
    {
        var dbContext = await GetDbContextAsync();
        return await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Name == name);
    }
    
}