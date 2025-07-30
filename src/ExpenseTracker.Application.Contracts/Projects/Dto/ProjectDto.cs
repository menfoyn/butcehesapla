using System;

namespace ExpenseTracker.Projects.Dto;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    
}