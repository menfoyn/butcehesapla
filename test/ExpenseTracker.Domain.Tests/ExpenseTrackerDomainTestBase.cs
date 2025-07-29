using Volo.Abp.Modularity;

namespace ExpenseTracker;

/* Inherit from this class for your domain layer tests. */
public abstract class ExpenseTrackerDomainTestBase<TStartupModule> : ExpenseTrackerTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
