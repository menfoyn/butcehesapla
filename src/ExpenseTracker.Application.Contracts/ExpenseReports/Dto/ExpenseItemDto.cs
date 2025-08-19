using System;
using System.Collections.Generic;

namespace ExpenseTracker.ExpenseReports;

public partial class ExpenseItemDto
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public List<string> ReceiptPaths { get; set; } = new();
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public string Currency { get; set; }
    public string? ReceiptImagePath { get; set; } = string.Empty;
    public string? ReceiptFilePath { get; set; }
    public string? ReceiptUrl { get; set; }
    public Dictionary<string, object> ExtraProperties { get; set; } = new();
    public string? Category { get; set; }
    public decimal WorkedHours { get; set; }
}