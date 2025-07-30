using System;
namespace ExpenseTracker.Projects.Dto;

public class CreateProjectDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
}