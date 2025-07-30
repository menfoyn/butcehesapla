using System;
namespace ExpenseTracker.ExpenseReports;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal? SpendingLimit { get; set; }
    public string? Description { get; set; }
}