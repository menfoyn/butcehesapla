using System;
using System.Collections.Generic;

namespace ExpenseTracker.ExpenseReports;

public class ExpenseReportDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public List<ExpenseItemDto> Items { get; set; } = new();
    public string Title { get; set; }
}