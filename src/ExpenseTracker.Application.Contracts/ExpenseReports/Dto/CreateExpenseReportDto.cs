using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.ExpenseReports;

public class CreateExpenseReportDto
{
    [Required(ErrorMessage = "Başlık zorunludur.")]
    public string Title { get; set; } = String.Empty;

    [Required(ErrorMessage = "Harcama limiti girilmelidir.")]
    public decimal SpendingLimit { get; set; }
    public string? ReceiptFilePath { get; set; }

    [Required(ErrorMessage = "Bir proje seçilmelidir.")]
    public Guid ProjectId { get; set; }

    public List<string> ReceiptPaths { get; set; } = new();

    [MinLength(1, ErrorMessage = "En az bir masraf kalemi girilmelidir.")]
    public List<CreateExpenseItemDto> Items { get; set; } = new();

    public Guid OwnerId { get; set; }
}