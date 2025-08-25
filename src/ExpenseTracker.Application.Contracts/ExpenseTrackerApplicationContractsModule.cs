using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using Volo.Abp.Authorization.Permissions;
using ExpenseTracker.Permissions;
using Volo.Abp.Authorization;

namespace ExpenseTracker;

[DependsOn(
    typeof(ExpenseTrackerDomainSharedModule),
    typeof(AbpAuthorizationModule)
)]
public class ExpenseTrackerApplicationContractsModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        ExpenseTrackerDtoExtensions.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpPermissionOptions>(options =>
        {
            options.DefinitionProviders.Add<ExpenseTrackerPermissionDefinitionProvider>();
        });
    }
}
