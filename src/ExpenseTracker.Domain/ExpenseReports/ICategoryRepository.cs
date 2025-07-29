using  System;
using Volo.Abp.Domain.Repositories;
using System.Threading.Tasks;

namespace ExpenseTracker.ExpenseReports;

public interface ICategoryRepository : IRepository<Category, Guid>
{
    Task<Category> FindByNameAsync(string name);
    
}