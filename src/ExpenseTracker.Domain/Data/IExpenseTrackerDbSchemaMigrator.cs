using System.Threading.Tasks;

namespace ExpenseTracker.Data;

public interface IExpenseTrackerDbSchemaMigrator
{
    Task MigrateAsync();
}
