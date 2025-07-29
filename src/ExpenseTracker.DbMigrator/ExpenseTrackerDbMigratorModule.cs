using ExpenseTracker.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace ExpenseTracker.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(ExpenseTrackerEntityFrameworkCoreModule),
    typeof(ExpenseTrackerApplicationContractsModule)
    )]
public class ExpenseTrackerDbMigratorModule : AbpModule
{
}
