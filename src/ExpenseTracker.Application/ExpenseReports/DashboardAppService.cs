using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.ExpenseReports;
using ExpenseTracker.Projects; // <— for Project repo
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using ExpenseTracker.Permissions;

namespace ExpenseTracker.Dashboard;

[Authorize(ExpenseTrackerPermissions.Dashboard.Default)]
public class DashboardAppService : ApplicationService, IDashboardAppService
{
    private readonly IRepository<ExpenseReport, Guid> _reportRepo;
    private readonly IRepository<ExpenseItem, Guid> _itemRepo;
    private readonly IRepository<Project, Guid> _projectRepo;
    private readonly IRepository<Category, Guid> _categoryRepo;

    public DashboardAppService(
        IRepository<ExpenseReport, Guid> reportRepo,
        IRepository<ExpenseItem, Guid> itemRepo,
        IRepository<Project, Guid> projectRepo,
        IRepository<Category, Guid> categoryRepo)
    {
        _reportRepo = reportRepo;
        _itemRepo = itemRepo;
        _projectRepo = projectRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task<DashboardDto> GetDashboardDataAsync()
    {
        return await GetDashboardDataAsync(null, null, null);
    }

    public async Task<DashboardDto> GetDashboardDataAsync(Guid? projectId, DateTime? dateFrom, DateTime? dateTo)
    {
        var reportQ  = await _reportRepo.GetQueryableAsync();
        var itemQ    = await _itemRepo.GetQueryableAsync();
        var projQ    = await _projectRepo.GetQueryableAsync();

        // Authorization-based filtering: non-admins see only their own reports
        var canViewAll = await AuthorizationService.IsGrantedAsync(ExpenseTrackerPermissions.Dashboard.ViewAll);
        if (!canViewAll)
        {
            var currentUserId = CurrentUser.GetId();
            reportQ = reportQ.Where(r => r.OwnerId == currentUserId);
        }

        // Only show APPROVED reports on the dashboard (hide Pending/Rejected)
        reportQ = reportQ.Where(r => r.Status == ExpenseReportStatus.Approved);

        // Apply explicit filters if provided
        if (projectId.HasValue && projectId.Value != Guid.Empty)
        {
            reportQ = reportQ.Where(r => r.ProjectId == projectId.Value);
        }
        if (dateFrom.HasValue)
        {
            var from = dateFrom.Value.Date;
            itemQ = itemQ.Where(i => i.Date >= from);
        }
        if (dateTo.HasValue)
        {
            // make `to` inclusive for the whole day
            var toExclusive = dateTo.Value.Date.AddDays(1);
            itemQ = itemQ.Where(i => i.Date < toExclusive);
        }

        // LEFT JOIN projects to pick the real project name if available
        var projectedQuery =
            from i in itemQ
            join r in reportQ on i.ExpenseReportId equals r.Id
            join p in projQ on r.ProjectId equals p.Id into gp
            from p in gp.DefaultIfEmpty()
            select new
            {
                ProjectId    = r.ProjectId,            // Guid
                ProjectName  = p != null ? p.Name : null,
                ItemDate     = i.Date,
                ItemAmount   = i.Amount,
                ItemHours    = i.WorkedHours,
                CategoryId   = i.CategoryId
            };

        var rows = await AsyncExecuter.ToListAsync(projectedQuery);

        // Aggregate budgets per project from reports (distinct from items)
        var reportLimits = await AsyncExecuter.ToListAsync(
            from r in reportQ
            group r by r.ProjectId into g
            select new { ProjectId = g.Key, TotalLimit = g.Sum(x => (decimal)x.SpendingLimit) }
        );
        var budgetByProject = reportLimits.ToDictionary(x => x.ProjectId, x => x.TotalLimit);

        // ---- Overall totals ----
        var totalInvoiced = rows.Sum(x => x.ItemAmount);
        var totalBudget   = reportLimits.Sum(x => x.TotalLimit);
        var totalHoursDec = rows.Sum(x => x.ItemHours);

        // ---- Weekly totals (last 8 weeks) ----
        var start = DateTime.UtcNow.Date.AddDays(-7 * 7);
        var weeklyDict = new Dictionary<string, (decimal actual, decimal invoiced, decimal hours)>();

        foreach (var x in rows)
        {
            if (x.ItemDate < start) continue;
            var week = ISOWeek.GetWeekOfYear(x.ItemDate);
            var key  = $"{x.ItemDate.Year}-W{week:D2}";
            if (!weeklyDict.TryGetValue(key, out var agg)) agg = (0m, 0m, 0m);
            agg.actual   += x.ItemAmount;
            agg.invoiced += x.ItemAmount;
            agg.hours    += x.ItemHours;
            weeklyDict[key] = agg;
        }

        var weeklyList = weeklyDict
            .OrderBy(kv => kv.Key)
            .Select(kv => new WeeklyChartDataDto
            {
                WeekLabel   = kv.Key,
                Actual      = kv.Value.actual,
                Invoiced    = kv.Value.invoiced,
                WorkedHours = (int)Math.Round(kv.Value.hours)
            })
            .ToList();

        // ---- Project cards ----
        var UNASSIGNED = Guid.Empty;
        var projectAgg = new Dictionary<Guid, (decimal amount, string? name, decimal hours)>();

        foreach (var x in rows)
        {
            var pid = (x.ProjectId == Guid.Empty ? UNASSIGNED : x.ProjectId);
            if (!projectAgg.TryGetValue(pid, out var agg)) agg = (0m, null, 0m);
            agg.amount += x.ItemAmount;
            agg.hours  += x.ItemHours;
            if (string.IsNullOrWhiteSpace(agg.name)) agg.name = x.ProjectName; // first non-empty name wins
            projectAgg[pid] = agg;
        }

        var projectCards = projectAgg
            .OrderByDescending(kv => kv.Value.amount)
            .Select(kv => new ProjectSummaryDto
            {
                ProjectId      = kv.Key,
                ProjectName    = !string.IsNullOrWhiteSpace(kv.Value.name)
                                   ? kv.Value.name!
                                   : (kv.Key == UNASSIGNED ? "Atanmamış" : kv.Key.ToString()),
                Budget         = budgetByProject.TryGetValue(kv.Key, out var b) ? b : 0m,
                InvoicedAmount = kv.Value.amount,
                WorkedHours    = (int)Math.Round(kv.Value.hours),
                TotalHours     = (int)Math.Round(kv.Value.hours)
            })
            .ToList();

        // ---- Category breakdown (all items) ----
        var catTotals = rows
            .GroupBy(x => x.CategoryId)
            .Select(g => new { CategoryId = g.Key, Amount = g.Sum(z => z.ItemAmount) })
            .ToList();

        var categories = await _categoryRepo.GetListAsync();
        var nameById = categories.ToDictionary(c => c.Id, c => c.Name);
        var totalCatAmount = catTotals.Sum(x => x.Amount);
        var catBreakdown = catTotals
            .OrderByDescending(x => x.Amount)
            .Select(x => new DashboardDto.CategoryBreakdownDto
            {
                Name    = (x.CategoryId != Guid.Empty && nameById.TryGetValue(x.CategoryId, out var nm)) ? nm : "Diğer",
                Amount  = x.Amount,
                Percent = totalCatAmount > 0 ? (int)Math.Round(x.Amount * 100m / totalCatAmount) : 0
            })
            .ToList();

        // ---- Series for the summary chart ----
        var weekLabels     = weeklyList.Select(w => w.WeekLabel).ToList();
        var actualSeries   = weeklyList.Select(w => w.Actual).ToList();
        var invoicedSeries = weeklyList.Select(w => w.Invoiced).ToList();
        var workedSeries   = weeklyList.Select(w => w.WorkedHours).ToList();

        // ---- Per‑project weekly breakdown ----
        var perProjectTmp = new Dictionary<Guid, Dictionary<string, (decimal actual, decimal invoiced, decimal hours)>>();

        foreach (var x in rows)
        {
            if (x.ItemDate < start) continue;
            var pid = (x.ProjectId == Guid.Empty ? UNASSIGNED : x.ProjectId);
            if (!perProjectTmp.TryGetValue(pid, out var weekMap))
            {
                weekMap = new Dictionary<string, (decimal actual, decimal invoiced, decimal hours)>();
                perProjectTmp[pid] = weekMap;
            }

            var week = ISOWeek.GetWeekOfYear(x.ItemDate);
            var key  = $"{x.ItemDate.Year}-W{week:D2}";
            if (!weekMap.TryGetValue(key, out var agg)) agg = (0m, 0m, 0m);
            agg.actual   += x.ItemAmount;
            agg.invoiced += x.ItemAmount;
            agg.hours    += x.ItemHours;
            weekMap[key] = agg;
        }

        var weeklyByProject = new Dictionary<Guid, List<WeeklyChartDataDto>>();
        foreach (var (pid, map) in perProjectTmp)
        {
            weeklyByProject[pid] = map
                .OrderBy(kv => kv.Key)
                .Select(kv => new WeeklyChartDataDto
                {
                    WeekLabel   = kv.Key,
                    Actual      = kv.Value.actual,
                    Invoiced    = kv.Value.invoiced,
                    WorkedHours = (int)Math.Round(kv.Value.hours)
                })
                .ToList();
        }

        return new DashboardDto
        {
            TotalBudget        = totalBudget,
            InvoicedAmount     = totalInvoiced,
            TotalHours         = (int)Math.Round((decimal)totalHoursDec),
            WorkedHours        = (int)Math.Round((decimal)totalHoursDec),
            ProjectSummaries   = projectCards,
            WeekLabels         = weekLabels,
            ActualSeries       = actualSeries,
            InvoicedSeries     = invoicedSeries,
            WorkedHoursSeries  = weeklyList.Select(w => w.WorkedHours).ToList(),
            WeeklyDataByProject = weeklyByProject,
            CategoryBreakdown   = catBreakdown,
        };
    }
}