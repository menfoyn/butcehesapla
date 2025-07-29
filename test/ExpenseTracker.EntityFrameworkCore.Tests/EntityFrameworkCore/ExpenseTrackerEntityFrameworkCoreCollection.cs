using Xunit;

namespace ExpenseTracker.EntityFrameworkCore;

[CollectionDefinition(ExpenseTrackerTestConsts.CollectionDefinitionName)]
public class ExpenseTrackerEntityFrameworkCoreCollection : ICollectionFixture<ExpenseTrackerEntityFrameworkCoreFixture>
{

}
