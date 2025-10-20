using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.Interceptors;
using Oduyo.Domain.Entities;

namespace Oduyo.DataAccess.DataContexts
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly AuditInterceptor _auditInterceptor;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            AuditInterceptor auditInterceptor) : base(options)
        {
            _auditInterceptor = auditInterceptor;
        }

        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserHierarchy> UserHierarchies { get; set; }
        public DbSet<DiscountAuthority> DiscountAuthorities { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<TenantUser> TenantUsers { get; set; }
        public DbSet<TenantSetting> TenantSettings { get; set; }
        public DbSet<Dealer> Dealers { get; set; }
        public DbSet<DealerUserMapping> DealerUserMappings { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyContact> CompanyContacts { get; set; }
        public DbSet<Demo> Demos { get; set; }
        public DbSet<DemoFeedback> DemoFeedbacks { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferDetail> OfferDetails { get; set; }
        public DbSet<OfferRevision> OfferRevisions { get; set; }
        public DbSet<OfferApproval> OfferApprovals { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<PackageModule> PackageModules { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<PackagePrice> PackagePrices { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignPackage> CampaignPackages { get; set; }
        public DbSet<CampaignModule> CampaignModules { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<LicenseHistory> LicenseHistories { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<VirtualPOS> VirtualPOSs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ReferenceCodeSequence> ReferenceCodeSequences { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Currency> Currencies { get; set; }

        // New entity DbSets
        public DbSet<DealerCommission> DealerCommissions { get; set; }
        public DbSet<DiscountRate> DiscountRates { get; set; }
        public DbSet<CreditUsage> CreditUsages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.AddInterceptors(_auditInterceptor);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureNewEntityRelationshipsAndIndexes(builder);
            ApplySoftDeleteFilters(builder);
            ApplyIdentitySoftDeleteFilters(builder);
            ApplyEntityIndexes(builder);
        }

        private static void ConfigureNewEntityRelationshipsAndIndexes(ModelBuilder builder)
        {
            // Campaign computed properties ignore
            builder.Entity<Campaign>()
                .Ignore(c => c.IsCurrentlyActive)
                .Ignore(c => c.IsWithinFreezePeriod);

            // New indexes for performance
            builder.Entity<DealerCommission>()
                .HasIndex(dc => new { dc.DealerId, dc.Status });

            builder.Entity<CreditUsage>()
                .HasIndex(cu => new { cu.CompanyId, cu.UsedAt });

            builder.Entity<Demo>()
                .HasIndex(d => d.ReferenceCode)
                .IsUnique();

            // Relationships
            builder.Entity<Offer>()
                .HasOne(o => o.Campaign)
                .WithMany()
                .HasForeignKey(o => o.CampaignId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void ApplyEntityIndexes(ModelBuilder builder)
        {
            // Existing indexes
            builder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            builder.Entity<Permission>().HasIndex(p => p.Code).IsUnique();
            builder.Entity<RolePermission>().HasIndex(rp => new { rp.RoleId, rp.PermissionId });
            builder.Entity<UserHierarchy>().HasIndex(uh => new { uh.ParentUserId, uh.ChildUserId });
            builder.Entity<DiscountAuthority>().HasIndex(da => da.UserId);
        }

        private static void ApplyIdentitySoftDeleteFilters(ModelBuilder builder)
        {
            // Identity User ve Role için DeletedAt filtresi
            builder.Entity<User>().HasQueryFilter(u => u.DeletedAt == null);
            builder.Entity<Role>().HasQueryFilter(r => r.DeletedAt == null);
        }

        private static void ApplySoftDeleteFilters(ModelBuilder builder)
        {
            // Existing soft delete filters
            builder.Entity<Permission>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<RolePermission>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<UserHierarchy>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<DiscountAuthority>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Tenant>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<TenantUser>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<TenantSetting>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Dealer>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<DealerUserMapping>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Company>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<CompanyContact>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Demo>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<DemoFeedback>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Offer>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<OfferDetail>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<OfferRevision>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<OfferApproval>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Product>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Package>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Module>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<PackageModule>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<ProductPrice>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<PackagePrice>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Campaign>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<CampaignPackage>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<CampaignModule>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<License>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<LicenseHistory>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Credit>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Payment>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<VirtualPOS>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Notification>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<AuditLog>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Setting>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<ReferenceCodeSequence>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<EmailTemplate>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Channel>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Status>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<Currency>().HasQueryFilter(e => e.DeletedAt == null);

            // New entity soft delete filters
            builder.Entity<DealerCommission>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<DiscountRate>().HasQueryFilter(e => e.DeletedAt == null);
            builder.Entity<CreditUsage>().HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
