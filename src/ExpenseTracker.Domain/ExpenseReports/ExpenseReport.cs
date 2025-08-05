using System;
using System.Collections.Generic;
using ExpenseTracker.ExpenseReports;
using Volo.Abp.Domain.Entities.Auditing;

public class ExpenseReport : FullAuditedAggregateRoot<Guid>
{
    public ExpenseReport(Guid projectId, decimal spendingLimit, DateTime createdAt, string? receiptFilePath, Guid? creatorId)
    {
        ProjectId = projectId;
        SpendingLimit = spendingLimit;
        CreatedAt = createdAt;
        ReceiptFilePath = receiptFilePath;
        CreatorId = creatorId;
    }

    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid ProjectId { get; set; }
    public decimal TotalAmount { get; set; }

    public decimal SpendingLimit { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ReceiptFilePath { get; set; }

    public Guid? CreatorId { get; set; }

    private List<ExpenseItem> _items = new List<ExpenseItem>();

    public ExpenseReport()
    {
        
    }

    public IReadOnlyList<ExpenseItem> Items => _items.AsReadOnly();
    public Guid OwnerId { get; set; }
    public Guid CreatorUserId { get; set; }

    public void AddItem(ExpenseItem item)
    {
        _items.Add(item);
    }

   
}