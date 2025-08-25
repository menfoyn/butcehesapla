using System;
using System.Collections;
using System.Collections.Generic;

namespace ExpenseTracker.Dashboard;

public class DashboardDto
{
    // Overall totals (all projects)
    public decimal TotalBudget { get; set; }
    public decimal InvoicedAmount { get; set; }
    public decimal RemainingAmount => TotalBudget - InvoicedAmount;

    public int TotalHours { get; set; }
    public int WorkedHours { get; set; }
    public int RemainingHours => TotalHours - WorkedHours;

    // Project cards (each project shown separately on the dashboard)
    public List<ProjectSummaryDto> ProjectSummaries { get; set; } = new();

    // Weekly data for the currently selected project (for simple charts)
    public List<string> WeekLabels { get; set; } = new();
    public List<decimal> InvoicedSeries { get; set; } = new();
    public List<decimal> ActualSeries { get; set; } = new();
    public List<int> WorkedHoursSeries { get; set; } = new();

    // If you need to show multiple projects on the same chart (stacked/compare),
    // fill this dictionary: key = ProjectId, value = weekly points for that project
    public Dictionary<Guid, List<WeeklyChartDataDto>> WeeklyDataByProject { get; set; } = new();
    public List<WeeklyChartDataDto> WeeklyData { get; set; } = new();
    public List<CategoryBreakdownDto> CategoryBreakdown { get; set; } = new();    public class CategoryBreakdownDto {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public int Percent { get; set; } // 0-100
    }
}

public class ProjectSummaryDto
{
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;

    public decimal Budget { get; set; }
    public decimal InvoicedAmount { get; set; }
    public decimal RemainingAmount => Budget - InvoicedAmount;

    public int TotalHours { get; set; }
    public int WorkedHours { get; set; }
    public int RemainingHours => TotalHours - WorkedHours;
}

public class WeeklyChartDataDto
{
    public string WeekLabel { get; set; } = string.Empty; // e.g. "2025-W32"
    public decimal Invoiced { get; set; }
    public decimal Actual { get; set; }
    public int WorkedHours { get; set; }
}