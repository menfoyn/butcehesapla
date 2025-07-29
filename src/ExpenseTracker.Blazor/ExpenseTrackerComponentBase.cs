using ExpenseTracker.Localization;
using Volo.Abp.AspNetCore.Components;

namespace ExpenseTracker.Blazor;

public abstract class ExpenseTrackerComponentBase : AbpComponentBase
{
    protected ExpenseTrackerComponentBase()
    {
        LocalizationResource = typeof(ExpenseTrackerResource);
    }
}
