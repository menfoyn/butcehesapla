using System;
using Volo.Abp.Domain.Entities;


namespace ExpenseTracker.Projects;

public class Project : Entity<Guid>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    
    public Project(){}

    public Project(Guid id, string name, string description)
    {
        Id = id;    
        Name = name;
        Description = description;
    }

    public Project(Guid create, string inputName)
    {
        throw new NotImplementedException();
    }
}