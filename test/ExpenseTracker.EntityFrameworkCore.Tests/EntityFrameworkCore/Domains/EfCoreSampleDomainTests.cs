using ExpenseTracker.Samples;
using Xunit;

namespace ExpenseTracker.EntityFrameworkCore.Domains;

[Collection(ExpenseTrackerTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<ExpenseTrackerEntityFrameworkCoreTestModule>
{

}
