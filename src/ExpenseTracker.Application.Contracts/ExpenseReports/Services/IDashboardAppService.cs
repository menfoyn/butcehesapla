using System;
using System.Threading.Tasks;
using ExpenseTracker.Dashboard;

namespace ExpenseTracker.Dashboard;

public interface IDashboardAppService
{
    Task<DashboardDto> GetDashboardDataAsync(Guid? uid);
}