using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using ExpenseTracker.Projects.Dto;
using ExpenseTracker.Projects;
using System;
using ExpenseTracker.Projects.Services;

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
        return projects.Select(x => new ProjectDto
        {
            Id = x.Id,
            Name = x.Name,
            Description = x.Description
        }).ToList();
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto input)
    {
        var project = new Project(GuidGenerator.Create(), input.Name)
        {
            Description = input.Description
        };

        await _repository.InsertAsync(project);
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description
        };
    }
}