using ExpenseTracker.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class ExpenseTrackerController : AbpControllerBase
{
    protected ExpenseTrackerController()
    {
        LocalizationResource = typeof(ExpenseTrackerResource);
    }
}
