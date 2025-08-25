using ExpenseTracker.Localization;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ExpenseTracker.Permissions;

public class ExpenseTrackerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        // 1) Create/resolve main permission group (idempotent)
        var group = context.GetGroupOrNull(ExpenseTrackerPermissions.GroupName)
                    ?? context.AddGroup(ExpenseTrackerPermissions.GroupName, L("Permission:ExpenseTracker"));

        // 2) Resolve or create the root permission for ExpenseReports (idempotent)
        var reports = context.GetPermissionOrNull(ExpenseTrackerPermissions.ExpenseReports.Default)
                      ?? group.AddPermission(
                            ExpenseTrackerPermissions.ExpenseReports.Default,
                            L("Permission:ExpenseReports")
                         );

        if (reports == null)
        {
            throw new AbpException("Failed to create or resolve the root permission: " + ExpenseTrackerPermissions.ExpenseReports.Default);
        }

        // 3) Resolve-or-create each child permission (idempotent)
        if (context.GetPermissionOrNull(ExpenseTrackerPermissions.ExpenseReports.Create) == null)
        {
            reports.AddChild(ExpenseTrackerPermissions.ExpenseReports.Create, L("Permission:Create"));
        }

        if (context.GetPermissionOrNull(ExpenseTrackerPermissions.ExpenseReports.Delete) == null)
        {
            reports.AddChild(ExpenseTrackerPermissions.ExpenseReports.Delete, L("Permission:Delete"));
        }

        if (context.GetPermissionOrNull(ExpenseTrackerPermissions.ExpenseReports.ViewAll) == null)
        {
            reports.AddChild(ExpenseTrackerPermissions.ExpenseReports.ViewAll, L("Permission:ViewAll"));
        }

        if (context.GetPermissionOrNull(ExpenseTrackerPermissions.ExpenseReports.Approve) == null)
        {
            reports.AddChild(ExpenseTrackerPermissions.ExpenseReports.Approve, L("Permission:Approve"));
        }

        // 4) Dashboard root permission (idempotent)
        var dashboard = context.GetPermissionOrNull(ExpenseTrackerPermissions.Dashboard.Default)
                         ?? group.AddPermission(
                                ExpenseTrackerPermissions.Dashboard.Default,
                                L("Permission:Dashboard")
                            );

        if (dashboard == null)
        {
            throw new AbpException("Failed to create or resolve the root permission: " + ExpenseTrackerPermissions.Dashboard.Default);
        }

        // Dashboard children (idempotent)
        if (context.GetPermissionOrNull(ExpenseTrackerPermissions.Dashboard.ViewAll) == null)
        {
            dashboard.AddChild(ExpenseTrackerPermissions.Dashboard.ViewAll, L("Permission:ViewAll"));
        }
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ExpenseTrackerResource>(name);
    }
}
