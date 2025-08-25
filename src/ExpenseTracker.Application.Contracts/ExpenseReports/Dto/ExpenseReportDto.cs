using System;
using System.Collections.Generic;

namespace ExpenseTracker.ExpenseReports;

public class ExpenseReportDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string? OwnerName { get; set; }

    public Guid ProjectId { get; set; }
    public decimal SpendingLimit { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }
    public string Title { get; set; }
    public List<ExpenseItemDto> Items { get; set; } = new();
    public string? ReceiptFilePath { get; set; }
    public string? ProjectName { get; set; }
    public string? OwnerUserName { get; set; }
}