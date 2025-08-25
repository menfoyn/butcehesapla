using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace ExpenseTracker.ExpenseReports
{
    public class ExpenseReport : FullAuditedAggregateRoot<Guid>
    {
        // EF parameterless ctor
        public ExpenseReport() { }

        // Domain-friendly ctor (Id ABP tarafından yönetilir; Id parametresi YOK)
        public ExpenseReport(Guid projectId, decimal spendingLimit, DateTime createdAt, string? receiptFilePath, Guid ownerId)
        {
            ProjectId = projectId;
            SpendingLimit = spendingLimit;
            CreatedAt = createdAt;
            ReceiptFilePath = receiptFilePath;
            OwnerId = ownerId;
            Status = "Pending";
            Title = string.Empty;
        }

        public ExpenseReport(Guid projectId, decimal spendingLimit, DateTime createdAt, string? receiptFilePath)
        {
            ProjectId = projectId;
            SpendingLimit = spendingLimit;
            CreatedAt = createdAt;
            ReceiptFilePath = receiptFilePath;

            Status = ExpenseReportStatus.Pending; // varsayılan
            Items = new List<ExpenseItem>();           }

        public string Title { get; set; } = string.Empty;
        public Guid ProjectId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal SpendingLimit { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ReceiptFilePath { get; set; }

        // 1-N ilişki (EF takip edebilsin diye virtual ICollection)
        public virtual ICollection<ExpenseItem> Items { get; set; } = new List<ExpenseItem>();

        public Guid OwnerId { get; set; }

        public void AddItem(ExpenseItem item) => Items.Add(item);
    }

    public static class ExpenseReportStatus
    {
        public const string Pending  = "Pending";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";    }
}