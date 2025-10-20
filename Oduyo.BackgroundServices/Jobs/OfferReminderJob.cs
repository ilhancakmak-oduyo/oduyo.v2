using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oduyo.DataAccess;
using Oduyo.Domain.Enums;
using Oduyo.Domain.Messages;

namespace Oduyo.BackgroundServices.Jobs
{
    public class OfferReminderJob
    {
        private readonly ApplicationDbContext _context;
        private readonly IBus _bus;
        private readonly ILogger<OfferReminderJob> _logger;

        public OfferReminderJob(ApplicationDbContext context, IBus bus, ILogger<OfferReminderJob> logger)
        {
            _context = context;
            _bus = bus;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            var reminderDate = DateTime.UtcNow.AddDays(5).Date;

            var offersNearingExpiry = await _context.Offers
                .Include(o => o.Company)
                .Where(o => o.Status == OfferStatus.Sent || o.Status == OfferStatus.Viewed)
                .Where(o => o.ValidUntil.Date == reminderDate)
                .ToListAsync();

            var messages = offersNearingExpiry.Select(offer => new SendEmailMessage
            {
                To = offer.Company.Email,
                Subject = "Teklif Hatırlatması",
                TemplateId = "offer-reminder",
                TemplateData = new Dictionary<string, string>
                {
                    ["OfferNo"] = offer.OfferNo,
                    ["ValidUntil"] = offer.ValidUntil.ToString("dd.MM.yyyy"),
                    ["DaysRemaining"] = "5"
                }
            }).ToList();

            // Bulk publish via MassTransit
            foreach (var message in messages)
            {
                await _bus.Publish(message);
            }

            _logger.LogInformation("Sent {Count} offer reminders", messages.Count);
        }
    }
}
