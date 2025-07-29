using Microsoft.Extensions.Localization;
using ExpenseTracker.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace ExpenseTracker.Blazor;

[Dependency(ReplaceServices = true)]
public class ExpenseTrackerBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<ExpenseTrackerResource> _localizer;

    public ExpenseTrackerBrandingProvider(IStringLocalizer<ExpenseTrackerResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
