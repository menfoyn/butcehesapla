using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace ExpenseTracker.DbMigrator;

public class AppIdentityDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IIdentityDataSeeder _identityDataSeeder;

    public AppIdentityDataSeedContributor(IIdentityDataSeeder identityDataSeeder)
    {
        _identityDataSeeder = identityDataSeeder;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        // ABP sürümleri arasında parametre isimleri değişebildiği için positional argüman kullandık.
        // Üçüncü argüman tenant içindir; tek tenant/host senaryosunda context.TenantId genelde null kalır.
        await _identityDataSeeder.SeedAsync(
            "admin@expensetracker.local",
            "1q2w3E*",
            context.TenantId
        );
    }
}