using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oduyo.DataAccess;
using Oduyo.Domain.Messages;

namespace Oduyo.BackgroundServices.Jobs
{
    public class LicenseMonitorJob
    {
        private readonly ApplicationDbContext _context;
        private readonly IBus _bus;
        private readonly ILogger<LicenseMonitorJob> _logger;

        public LicenseMonitorJob(ApplicationDbContext context, IBus bus, ILogger<LicenseMonitorJob> logger)
        {
            _context = context;
            _bus = bus;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            // 1. Activate grace period for expired licenses
            var expiredLicenses = await _context.Licenses
                .Include(l => l.Company)
                .Where(l => l.IsActive && !l.IsTrial)
                .Where(l => DateTime.UtcNow > l.EndDate)
                .Where(l => l.GracePeriodStartDate == null)
                .ToListAsync();

            foreach (var license in expiredLicenses)
            {
                license.GracePeriodStartDate = DateTime.UtcNow;

                await _bus.Publish(new SendEmailMessage
                {
                    To = license.Company.Email,
                    Subject = "Lisans Süresi Doldu",
                    TemplateId = "license-expired",
                    TemplateData = new Dictionary<string, string>
                    {
                        ["ProductName"] = license.Product?.Name ?? "Ürün",
                        ["GraceDays"] = license.GracePeriodDays.ToString()
                    }
                });
            }

            await _context.SaveChangesAsync();

            // 2. Deactivate licenses after grace period
            var gracePeriodEnded = await _context.Licenses
                .Include(l => l.Company)
                .Where(l => l.IsActive && l.GracePeriodStartDate.HasValue)
                .ToListAsync();

            int deactivatedCount = 0;
            foreach (var license in gracePeriodEnded)
            {
                var graceEndDate = license.GracePeriodStartDate.Value.AddDays(license.GracePeriodDays);

                if (DateTime.UtcNow > graceEndDate)
                {
                    license.IsActive = false;
                    deactivatedCount++;

                    await _bus.Publish(new SendEmailMessage
                    {
                        To = license.Company.Email,
                        Subject = "Lisans Devre Dışı",
                        TemplateId = "license-deactivated",
                        TemplateData = new Dictionary<string, string>
                        {
                            ["ProductName"] = license.Product?.Name ?? "Ürün"
                        }
                    });
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Activated grace period for {ExpiredCount} licenses, deactivated {DeactivatedCount}",
                expiredLicenses.Count, deactivatedCount
            );
        }
    }
}
