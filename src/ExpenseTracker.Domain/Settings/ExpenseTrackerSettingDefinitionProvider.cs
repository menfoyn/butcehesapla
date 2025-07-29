using Volo.Abp.Settings;

namespace ExpenseTracker.Settings;

public class ExpenseTrackerSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(ExpenseTrackerSettings.MySetting1));
    }
}
