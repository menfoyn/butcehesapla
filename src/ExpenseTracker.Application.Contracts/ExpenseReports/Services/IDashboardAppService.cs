using System.Threading.Tasks;

namespace ExpenseTracker.ExpenseReports;

public interface IDashboardAppService
{
    Task<DashboardDto> GetDashboardDataAsync();
}