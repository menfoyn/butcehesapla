using System;
using Volo.Abp.Domain.Entities;

namespace ExpenseTracker.ExpenseReports;

public class ExpenseItem : Entity<Guid>
{
    public Guid ExpenseReportId { get; set; }
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public string? ReceiptImagePath { get; set; }
    public string Currency { get; set; }
    
}