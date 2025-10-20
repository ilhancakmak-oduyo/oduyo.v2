using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oduyo.DataAccess;
using Oduyo.Domain.Constants;
using Oduyo.Domain.Enums;

namespace Oduyo.BackgroundServices.Jobs
{
    public class OfferExpiryJob
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OfferExpiryJob> _logger;

        public OfferExpiryJob(ApplicationDbContext context, ILogger<OfferExpiryJob> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            var expiredOffers = await _context.Offers
                .Where(o => o.Status == OfferStatus.Sent || o.Status == OfferStatus.Viewed)
                .Where(o => o.ValidUntil < DateTime.UtcNow)
                .ToListAsync();

            foreach (var offer in expiredOffers)
            {
                try
                {
                    offer.Status = OfferStatus.Expired;
                    offer.SetUpdated(SystemUserId.BackgroundJob);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to expire offer {OfferId}", offer.Id);
                }
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Expired {Count} offers", expiredOffers.Count);
        }
    }
}
