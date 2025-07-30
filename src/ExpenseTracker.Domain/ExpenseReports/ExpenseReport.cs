using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;


namespace ExpenseTracker.ExpenseReports;

public class ExpenseReport : FullAuditedAggregateRoot<Guid>
{
    public Guid OwnerId { get; set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; set; }
    public Guid? ProjectId { get; set; }
    
    
    public ICollection<ExpenseItem> Items { get; set; }

    public ExpenseReport()
    {
        Items = new List<ExpenseItem>();
    }

    public void AddItem(ExpenseItem item)
    {
        Items.Add(item);
        CalculateTotal();
        
    }

    public void CalculateTotal()
    {
        TotalAmount = 0;
        foreach (var item in Items)
        {
            TotalAmount += item.Amount;
        }
    }
    
}
