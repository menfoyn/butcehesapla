using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ExpenseTracker.Data;

/* This is used if database provider does't define
 * IExpenseTrackerDbSchemaMigrator implementation.
 */
public class NullExpenseTrackerDbSchemaMigrator : IExpenseTrackerDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
