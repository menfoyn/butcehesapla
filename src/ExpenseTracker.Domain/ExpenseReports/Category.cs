using System;
using Volo.Abp.Domain.Entities;

namespace ExpenseTracker.ExpenseReports;

public class Category : Entity<Guid>
{
    public string Name { get; set; }
    public decimal? SpendingLimit { get; set; }
    public string? Description { get; set; }

    // EF için parametresiz kurucu (public ya da protected olabilir)
    public Category() { }

    // 🔧 ÖNEMLİ: Parametre adı 'id' olmalı
    public Category(Guid id)
    {
        Id = id;
    }
}