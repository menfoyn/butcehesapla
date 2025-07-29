using System;
using Volo.Abp.Domain.Entities;

namespace ExpenseTracker.ExpenseReports;

    public class Category : Entity<Guid>

    {
        public string Name { get; set; }
        public decimal? SpendingLimit { get; set; }
        public string? Description { get; set; }

    }
