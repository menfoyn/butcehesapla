using AutoMapper;
using ExpenseTracker.ExpenseReports;
using ExpenseTracker.Projects;
using ExpenseTracker.Projects.Dto;

namespace ExpenseTracker;

public class ExpenseTrackerApplicationAutoMapperProfile : Profile
{
    public ExpenseTrackerApplicationAutoMapperProfile()
    {
        CreateMap<ExpenseReport, ExpenseReportDto>();
        CreateMap<ExpenseItem, ExpenseItemDto>()
            .ForMember(dest => dest.ReceiptPaths, opt => opt.MapFrom(src => src.ReceiptPaths));
        CreateMap<CreateExpenseItemDto, ExpenseItem>()
            .ForMember(dest => dest.ReceiptPaths, opt => opt.MapFrom(src => src.ReceiptPaths));        CreateMap<Category, CategoryDto>();
        CreateMap<Project, ProjectDto>();
        CreateMap<CreateProjectDto, Project>();
        CreateMap<CreateExpenseReportDto, ExpenseReport>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
