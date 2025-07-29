using Volo.Abp.Modularity;

namespace ExpenseTracker;

[DependsOn(
    typeof(ExpenseTrackerDomainModule),
    typeof(ExpenseTrackerTestBaseModule)
)]
public class ExpenseTrackerDomainTestModule : AbpModule
{

}
