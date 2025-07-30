using System;
using System.Collections.Generic;

namespace ExpenseTracker.ExpenseReports
{
    public class ExpenseReport
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid ProjectId { get; set; }
        public decimal SpendingLimit { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }

        private List<ExpenseItem> _items = new List<ExpenseItem>();
        public IReadOnlyList<ExpenseItem> Items => _items.AsReadOnly();

        public void AddItem(ExpenseItem item)
        {
            _items.Add(item);
        }
    }
}