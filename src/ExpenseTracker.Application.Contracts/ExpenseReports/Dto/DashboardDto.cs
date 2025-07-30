using System.Collections.Generic;

namespace ExpenseTracker.ExpenseReports;

public class DashboardDto
{
    public decimal TotalBudget { get; set; }
    public decimal InvoicedAmount { get; set; }
    public decimal RemainingAmount => TotalBudget - InvoicedAmount;

    public int TotalHours { get; set; }
    public int WorkedHours { get; set; }
    public int RemainingHours => TotalHours - WorkedHours;

    public List<WeeklyChartDataDto> WeeklyData { get; set; } = new();
}

public class WeeklyChartDataDto
{
    public string WeekLabel { get; set; } = string.Empty;
    public decimal Invoiced { get; set; }
    public decimal Actual { get; set; }
    public int WorkedHours { get; set; }
}