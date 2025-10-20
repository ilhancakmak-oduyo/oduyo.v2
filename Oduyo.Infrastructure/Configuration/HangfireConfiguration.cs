using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;
using Oduyo.Infrastructure.Features;

namespace Oduyo.Infrastructure.Configuration
{
    /// <summary>
    /// Hangfire yapılandırma extension methods
    /// </summary>
    public static class HangfireConfiguration
    {
        /// <summary>
        /// Hangfire servislerini DI container'a ekler
        /// </summary>
        public static IServiceCollection AddHangfireServices(
            this IServiceCollection services,
            string connectionString)
        {
            // Hangfire'ı PostgreSQL ile yapılandır
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UsePostgreSqlStorage(options =>
                    options.UseNpgsqlConnection(connectionString)));

            // Hangfire server'ı ekle
            services.AddHangfireServer(options =>
            {
                options.WorkerCount = Environment.ProcessorCount * 5;
                options.Queues = new[] { "critical", "default", "low" };
            });

            // Background job servisini ekle
            services.AddScoped<IBackgroundJobService, BackgroundJobService>();

            return services;
        }

        /// <summary>
        /// Recurring job'ları planlar
        /// </summary>
        public static void ScheduleRecurringJobs()
        {
            // Her gece 02:00'de süresi dolmuş kampanyaları işle
            RecurringJob.AddOrUpdate<IBackgroundJobService>(
                "process-expired-campaigns",
                service => service.ProcessExpiredCampaignsAsync(),
                "0 2 * * *", // Cron: Her gün saat 02:00
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time")
                });

            // Her gün 09:00'da teklif hatırlatıcıları gönder
            RecurringJob.AddOrUpdate<IBackgroundJobService>(
                "send-offer-reminders",
                service => service.SendOfferRemindersAsync(),
                "0 9 * * *", // Cron: Her gün saat 09:00
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time")
                });

            // Her 10 dakikada bir OTP kodlarını temizle
            RecurringJob.AddOrUpdate<IBackgroundJobService>(
                "cleanup-otp-codes",
                service => service.CleanupOtpCodesAsync(),
                "*/10 * * * *"); // Cron: Her 10 dakikada

            // Her ayın ilk günü 03:00'te bayi komisyon raporları oluştur
            RecurringJob.AddOrUpdate<IBackgroundJobService>(
                "generate-dealer-commission-reports",
                service => service.GenerateDealerCommissionReportsAsync(),
                "0 3 1 * *", // Cron: Her ayın 1. günü saat 03:00
                new RecurringJobOptions
                {
                    TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time")
                });
        }
    }
}
