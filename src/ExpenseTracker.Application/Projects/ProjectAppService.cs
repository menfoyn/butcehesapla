using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Projects.Dto;
using ExpenseTracker.Projects.Services;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ExpenseTracker.Projects;

public class ProjectAppService : ApplicationService, IProjectAppService
{
    private readonly IRepository<Project, Guid> _repository;

    public ProjectAppService(IRepository<Project, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<List<ProjectDto>> GetListAsync()
    {
        var projects = await _repository.GetListAsync();
        return projects
            .OrderBy(x => x.Name)
            .Select(x => new ProjectDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            })
            .ToList();
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto input)
    {
        var entity = new Project(GuidGenerator.Create(), input.Name)
        {
            Description = input.Description
        };

        await _repository.InsertAsync(entity, autoSave: true);

        return new ProjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description
        };
    }

    /// <summary>
    /// If there is no project, seeds a few sample ones once; otherwise returns the first project.
    /// </summary>
    public async Task<ProjectDto> CreateSampleProjectAsync()
    {
        // Desired default projects (idempotent seeding)
        var desired = new[]
        {
            new { Name = "Şehir Dışı",            Description = "Toplantı" },
            new { Name = "Toplantı",               Description = "Eğitim projeleri" },
            new { Name = "Yazılım Geliştirme", Description = "Uygulama ve servis geliştirme" }
        };

        // Get current projects once
        var existing = await _repository.GetListAsync();

        // Insert only the missing ones (by Name, case-insensitive)
        var names = new HashSet<string>(existing.Select(x => x.Name), StringComparer.OrdinalIgnoreCase);
        var toInsert = new List<Project>();

        foreach (var d in desired)
        {
            if (!names.Contains(d.Name))
            {
                toInsert.Add(new Project(GuidGenerator.Create(), d.Name, d.Description));
            }
        }

        if (toInsert.Count > 0)
        {
            await _repository.InsertManyAsync(toInsert, autoSave: true);
            // refresh existing list to include newly inserted rows
            existing = await _repository.GetListAsync();
        }

        // Return a deterministic project so caller can bind or select something
        var first = existing.OrderBy(x => x.Name).First();
        return new ProjectDto
        {
            Id = first.Id,
            Name = first.Name,
            Description = first.Description
        };
    }
}