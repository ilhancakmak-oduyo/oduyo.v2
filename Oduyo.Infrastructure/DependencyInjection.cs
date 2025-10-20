using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oduyo.Infrastructure.Communication;
using Oduyo.Infrastructure.Configuration;
using Oduyo.Infrastructure.Features;
using Oduyo.Infrastructure.Implementations;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure
{
    /// <summary>
    /// Infrastructure katmanı servislerini DI container'a ekleyen extension methods
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Tüm Infrastructure servislerini ekler
        /// </summary>
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration,
            bool useInMemoryMassTransit = false)
        {
            // Connection String
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // ⭐ Hangfire
            services.AddHangfireServices(connectionString);

            // ⭐ MassTransit
            if (useInMemoryMassTransit)
            {
                services.AddMassTransitInMemory();
            }
            else
            {
                services.AddMassTransitServices(configuration);
            }

            // Communication Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISmsService, SmsService>();

            // Google Calendar Configuration & Service
            services.Configure<GoogleCalendarConfig>(
                configuration.GetSection(GoogleCalendarConfig.SectionName));
            services.AddScoped<ICalendarService, CalendarService>();

            // Background Services
            services.AddScoped<IBackgroundJobService, BackgroundJobService>();

            // User & Auth Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IUserHierarchyService, UserHierarchyService>();
            services.AddScoped<IDiscountAuthorityService, DiscountAuthorityService>();

            // Tenant Services
            services.AddScoped<ITenantService, TenantService>();
            services.AddScoped<ITenantUserService, TenantUserService>();
            services.AddScoped<ITenantSettingService, TenantSettingService>();

            // Dealer Services
            services.AddScoped<IDealerService, DealerService>();
            services.AddScoped<IDealerUserMappingService, DealerUserMappingService>();

            // Company Services
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICompanyContactService, CompanyContactService>();

            // Demo Services
            services.AddScoped<IDemoService, DemoService>();
            services.AddScoped<IDemoFeedbackService, DemoFeedbackService>();

            // Offer Services
            services.AddScoped<IOfferService, OfferService>();
            services.AddScoped<IOfferDetailService, OfferDetailService>();
            services.AddScoped<IOfferRevisionService, OfferRevisionService>();
            services.AddScoped<IOfferApprovalService, OfferApprovalService>();
            services.AddScoped<IOfferPricingService, OfferPricingService>();

            // Product & Package Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductPriceService, ProductPriceService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<IPackagePriceService, PackagePriceService>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IPackageModuleService, PackageModuleService>();

            // Campaign Services
            services.AddScoped<ICampaignService, CampaignService>();
            services.AddScoped<ICampaignPackageService, CampaignPackageService>();
            services.AddScoped<ICampaignModuleService, CampaignModuleService>();

            // License & Credit Services
            services.AddScoped<ILicenseService, LicenseService>();
            services.AddScoped<ILicenseHistoryService, LicenseHistoryService>();
            services.AddScoped<ICreditService, CreditService>();

            // Payment Services
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IVirtualPOSService, VirtualPOSService>();

            // Notification & Audit Services
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAuditLogService, AuditLogService>();

            // Lookup Services
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IReferenceCodeService, ReferenceCodeService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<ICurrencyService, CurrencyService>();

            return services;
        }
    }
}
