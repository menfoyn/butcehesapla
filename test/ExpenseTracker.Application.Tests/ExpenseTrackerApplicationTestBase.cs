using Volo.Abp.Modularity;

namespace ExpenseTracker;

public abstract class ExpenseTrackerApplicationTestBase<TStartupModule> : ExpenseTrackerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
