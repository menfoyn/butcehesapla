using ExpenseTracker.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace ExpenseTracker.Permissions;

public class ExpenseTrackerPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ExpenseTrackerPermissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(ExpenseTrackerPermissions.MyPermission1, L("Permission:MyPermission1"));
        myGroup.AddPermission(ExpenseTrackerPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ExpenseTrackerResource>(name);
    }
}
