using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oduyo.DataAccess;
using Oduyo.Domain.Messages;

namespace Oduyo.BackgroundServices.Jobs
{
    public class CreditExpiryJob
    {
        private readonly ApplicationDbContext _context;
        private readonly IBus _bus;
        private readonly ILogger<CreditExpiryJob> _logger;

        public CreditExpiryJob(ApplicationDbContext context, IBus bus, ILogger<CreditExpiryJob> logger)
        {
            _context = context;
            _bus = bus;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            // Expiring today
            var expiringToday = await _context.Credits
                .Include(c => c.Company)
                .Where(c => c.ExpiryDate.Date == DateTime.UtcNow.Date)
                .Where(c => c.RemainingCredit > 0)
                .ToListAsync();

            foreach (var credit in expiringToday)
            {
                await _bus.Publish(new SendEmailMessage
                {
                    To = credit.Company.Email,
                    Subject = "Kredi Süresi Doldu",
                    TemplateId = "credit-expired",
                    TemplateData = new Dictionary<string, string>
                    {
                        ["UnusedCredit"] = credit.RemainingCredit.ToString()
                    }
                });
            }

            // Warning 7 days before
            var warningDate = DateTime.UtcNow.AddDays(7).Date;
            var expiringIn7Days = await _context.Credits
                .Include(c => c.Company)
                .Where(c => c.ExpiryDate.Date == warningDate)
                .Where(c => c.RemainingCredit > 0)
                .ToListAsync();

            foreach (var credit in expiringIn7Days)
            {
                await _bus.Publish(new SendEmailMessage
                {
                    To = credit.Company.Email,
                    Subject = "Kredi Hatırlatması",
                    TemplateId = "credit-expiring-soon",
                    TemplateData = new Dictionary<string, string>
                    {
                        ["RemainingCredit"] = credit.RemainingCredit.ToString(),
                        ["DaysRemaining"] = "7"
                    }
                });
            }

            _logger.LogInformation(
                "Processed {ExpiredCount} expired credits, sent {WarningCount} warnings",
                expiringToday.Count, expiringIn7Days.Count
            );
        }
    }
}
