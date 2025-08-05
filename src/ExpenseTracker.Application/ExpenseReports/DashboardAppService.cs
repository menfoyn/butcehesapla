using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;
using ExpenseTracker.Dashboard;
using ExpenseTracker.ExpenseReports;

namespace ExpenseTracker.Dashboard;

public class DashboardAppService : ApplicationService, IDashboardAppService
{
    private readonly IExpenseReportRepository _expenseReportRepository;
    private readonly ICurrentUser _currentUser;

    public DashboardAppService(IExpenseReportRepository expenseReportRepository, ICurrentUser currentUser)
    {
        _expenseReportRepository = expenseReportRepository;
        _currentUser = currentUser;
    }

    public async Task<DashboardDto> GetDashboardDataAsync()
    {
        var reports = await _expenseReportRepository.GetListAsync();

        if (!_currentUser.IsInRole("Admin"))
        {
            reports = reports.Where(r => r.CreatorId == _currentUser.Id).ToList();
        }

        var totalBudget = reports.Sum(r => r.TotalAmount);
        var invoicedAmount = reports.Where(r => r.Status == "Invoiced").Sum(r => r.TotalAmount);
        var workedHours = reports.SelectMany(r => r.Items).Sum(i => i.WorkedHours);
        var totalHours = 100; // Sabit, istersen dinamikleştirirsin

        var grouped = reports.GroupBy(r =>
            CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                r.CreationTime,
                CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday)
        );

        var weeklyData = grouped.Select(g => new WeeklyChartDataDto
        {
            WeekLabel = $"W{g.Key}",
            Actual = g.Sum(r => r.TotalAmount),
            Invoiced = g.Where(r => r.Status == "Invoiced").Sum(r => r.TotalAmount),
            WorkedHours = g.SelectMany(r => r.Items).Sum(i => i.WorkedHours)
        }).ToList();

        return new DashboardDto
        {
            TotalBudget = totalBudget,
            InvoicedAmount = invoicedAmount,
            TotalHours = totalHours,
            WorkedHours = workedHours,
            WeeklyData = weeklyData
        };
    }
}