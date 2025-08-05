using ExpenseTracker.Localization;
using Volo.Abp.Application.Services;

namespace ExpenseTracker;

/* Inherit your application services from this class.
 */
public abstract class ExpenseTrackerAppService : ApplicationService
{
    protected ExpenseTrackerAppService()
    {
        LocalizationResource = typeof(ExpenseTrackerResource);
    }
}