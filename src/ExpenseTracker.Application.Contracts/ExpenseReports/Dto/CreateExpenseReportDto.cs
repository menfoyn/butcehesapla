using System;
using System.Collections.Generic;

namespace ExpenseTracker.ExpenseReports;

public class CreateExpenseReportDto
{
    public string Title { get; set; }
    public decimal? SpendingLimit { get; set; }
    public Guid? ProjectId { get; set; }
    public List<string> ReceiptPaths { get; set; } = new();
    public List<CreateExpenseItemDto> Items { get; set; } = new();
    public Guid OwnerId { get; set; }
}