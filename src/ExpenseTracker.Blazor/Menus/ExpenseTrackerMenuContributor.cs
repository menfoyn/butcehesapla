using System.Threading.Tasks;
using ExpenseTracker.Localization;
using ExpenseTracker.MultiTenancy;
using Volo.Abp.Identity.Blazor;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.UI.Navigation;

namespace ExpenseTracker.Blazor.Menus;

public class ExpenseTrackerMenuContributor : IMenuContributor
{
    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var administration = context.Menu.GetAdministration();
        var l = context.GetLocalizer<ExpenseTrackerResource>();

        context.Menu.Items.Insert(
            0,
            new ApplicationMenuItem(
                ExpenseTrackerMenus.Home,
                l["Menu:Home"],
                "/",
                icon: "fas fa-home",
                order: 0
            )
        );

        context.Menu.AddItem(
            new ApplicationMenuItem(
                "ExpenseReports",
                l["Menu:ExpenseReports"],
                "/expense-reports",
                icon: "fas fa-file-invoice-dollar"
            )
        );
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "CreateExpenseReport",
                "Yeni Masraf Raporu",
                "/expense-reports/create",
                icon: "fa fa-plus"
            )
        );
        context.Menu.AddItem(new ApplicationMenuItem(
            "Dashboard",
            l["Menu:Dashboard"],
            "/dashboard",
            icon: "fas fa-chart-line"
        ));

        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);

        return Task.CompletedTask;
    }
    
}
