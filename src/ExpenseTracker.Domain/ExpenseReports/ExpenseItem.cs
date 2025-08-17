using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace ExpenseTracker.ExpenseReports;

public class ExpenseItem : Entity<Guid>
{
    public Guid ExpenseReportId { get; set; }
    public DateTime Date { get; set; }
    public  decimal Amount { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public string? ReceiptImagePath { get; set; }
    public string? ReceiptPath { get; set; }

    public string? ReceiptPathsJson { get; set; }

    [NotMapped]
    public List<string> ReceiptPaths
    {
        get => string.IsNullOrWhiteSpace(ReceiptPathsJson)
            ? new List<string>()
            : System.Text.Json.JsonSerializer.Deserialize<List<string>>(ReceiptPathsJson);
        set => ReceiptPathsJson = System.Text.Json.JsonSerializer.Serialize(value ?? new List<string>());
    }
    public int WorkedHours { get; set; }
    public string Currency { get; set; }
    public string Name { get; set; }
}