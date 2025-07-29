using Volo.Abp.Modularity;

namespace ExpenseTracker;

[DependsOn(
    typeof(ExpenseTrackerApplicationModule),
    typeof(ExpenseTrackerDomainTestModule)
)]
public class ExpenseTrackerApplicationTestModule : AbpModule
{

}
