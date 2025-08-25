namespace ExpenseTracker.Permissions;

public static class ExpenseTrackerPermissions
{
    public const string GroupName = "ExpenseTracker";

    public static class ExpenseReports
    {
        public const string Default = GroupName + ".ExpenseReports";   // Görüntüleme/Listeleme (kendi kayıtları)
        public const string Create  = Default + ".Create";             // Oluşturma
        public const string Delete  = Default + ".Delete";             // Silme (kendi kaydı)
        public const string ViewAll = Default + ".ViewAll";            // Tüm kayıtları görme (admin/manager)
        public const string Approve = Default + ".Approve";            // Onay/Red verme
        public static string Reject { get; set; }
    }

    public static class Dashboard
    {
        public const string Default = GroupName + ".Dashboard";
        public const string ViewAll = Default + ".ViewAll";
    }
}
