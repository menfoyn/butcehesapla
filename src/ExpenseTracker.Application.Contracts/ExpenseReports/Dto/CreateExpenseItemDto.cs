using System;
using System.Collections.Generic;
namespace ExpenseTracker.ExpenseReports;

public class CreateExpenseItemDto
{
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public string Category {get; set; }
    public int WorkedHours { get; set; }
    public List<string> ReceiptPaths { get; set; } = new();

    public string Currency { get; set; }
    public string? ReceiptImagePath { get; set; }
}