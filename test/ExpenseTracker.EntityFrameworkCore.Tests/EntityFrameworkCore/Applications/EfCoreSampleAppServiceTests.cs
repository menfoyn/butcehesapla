using ExpenseTracker.Samples;
using Xunit;

namespace ExpenseTracker.EntityFrameworkCore.Applications;

[Collection(ExpenseTrackerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<ExpenseTrackerEntityFrameworkCoreTestModule>
{

}
