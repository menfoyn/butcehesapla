using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using ExpenseTracker.Projects;

using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using ExpenseTracker.ExpenseReports;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ExpenseTracker.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class ExpenseTrackerDbContext :
    AbpDbContext<ExpenseTrackerDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion
    public DbSet<ExpenseReport> ExpenseReports { get; set; }
    public DbSet<ExpenseItem> ExpenseItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Project> Projects { get; set; }
    public ExpenseTrackerDbContext(DbContextOptions<ExpenseTrackerDbContext> options)
        : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(ExpenseTrackerConsts.DbTablePrefix + "YourEntities", ExpenseTrackerConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
        builder.Entity<Project>(b =>
        {
            b.ToTable("AppProjects");
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(500);
        });

        builder.Entity<ExpenseReport>(b =>
        {
            b.ToTable("ExpenseReports");
            b.ConfigureByConvention(); // Includes audit fields like CreatorId

            b.Property(x => x.Title).IsRequired().HasMaxLength(256);
            b.Property(x => x.Status).HasMaxLength(64);
            b.Property(x => x.SpendingLimit).HasColumnType("decimal(18,2)");
            b.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");

            b.HasMany(e => e.Items)
                .WithOne()
                .HasForeignKey(i => i.ExpenseReportId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Make EF access the collection via the backing field (e.g., _items)
            b.Navigation(e => e.Items)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        });

        builder.Entity<ExpenseItem>(b =>
        {
            b.ToTable("ExpenseItems");
            b.ConfigureByConvention();

            b.Property(x => x.Amount).HasColumnType("decimal(18,2)");
            b.Property(x => x.Currency).HasMaxLength(8);
            b.Property(x => x.Name).HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(1024);

            // 🔗 Category FK (Kategori zorunlu olsun istiyorsan alttaki satırı aç)
            // b.Property(x => x.CategoryId).IsRequired();

            b.HasOne<Category>()
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // kategori silinince item’lar bozulmasın

            // ⚡ Performans için indexler
            b.HasIndex(x => x.CategoryId);
            b.HasIndex(x => x.ExpenseReportId);
        });

        builder.Entity<Category>(b =>
        {
            b.ToTable("Categories");
            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.SpendingLimit).HasColumnType("decimal(18,2)");
        });
    }
}