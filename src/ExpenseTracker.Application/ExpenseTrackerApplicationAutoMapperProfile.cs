using AutoMapper;
using ExpenseTracker.ExpenseReports;

namespace ExpenseTracker;

public class ExpenseTrackerApplicationAutoMapperProfile : Profile
{
    public ExpenseTrackerApplicationAutoMapperProfile()
    {
        CreateMap<ExpenseReport, ExpenseReportDto>();
        CreateMap<ExpenseItem, ExpenseItemDto>();
        CreateMap<Category, CategortDto>();
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
    }
}
