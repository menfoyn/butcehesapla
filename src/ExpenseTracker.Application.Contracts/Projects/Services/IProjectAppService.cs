using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Projects.Dto;
using Volo.Abp.Application.Services;


namespace ExpenseTracker.Projects.Services;

public interface IProjectAppService:IApplicationService

{
    Task<List<ProjectDto>> GetListAsync();
    Task<ProjectDto> CreateAsync(CreateProjectDto input);
}