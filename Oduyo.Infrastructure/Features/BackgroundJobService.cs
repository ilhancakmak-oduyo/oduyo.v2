using Hangfire;
using Microsoft.Extensions.Logging;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Oduyo.Infrastructure.Features
{
    public interface IBackgroundJobService
    {
        Task ProcessExpiredCampaignsAsync();
        Task SendOfferRemindersAsync();
        Task CleanupOtpCodesAsync();
        Task GenerateDealerCommissionReportsAsync();
    }

    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BackgroundJobService> _logger;

        public BackgroundJobService(ApplicationDbContext context, ILogger<BackgroundJobService> logger)
        {
            _context = context;
            _logger = logger;
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task ProcessExpiredCampaignsAsync()
        {
            _logger.LogInformation("Starting expired campaigns processing job");

            try
            {
                var expiredCampaigns = await _context.Campaigns
                    .Where(c => c.IsActive && c.EndDate < DateTime.UtcNow)
                    .ToListAsync();

                foreach (var campaign in expiredCampaigns)
                {
                    campaign.IsActive = false;
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Processed {Count} expired campaigns", expiredCampaigns.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing expired campaigns");
                throw;
            }
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task SendOfferRemindersAsync()
        {
            _logger.LogInformation("Starting offer reminders job");

            try
            {
                var reminderDate = DateTime.UtcNow.AddDays(3);
                var offersNearExpiry = await _context.Offers
                    .Include(o => o.Company)
                    .Where(o => o.Status == OfferStatus.Sent &&
                               o.ValidUntil.Date == reminderDate.Date)
                    .ToListAsync();

                foreach (var offer in offersNearExpiry)
                {
                    // Create notification
                    var notification = new Notification
                    {
                        Title = "Teklif Süresi Yaklaşıyor",
                        Message = $"{offer.OfferNo} numaralı teklifinizin geçerlilik süresi 3 gün içinde dolacak.",
                        Type = NotificationType.Warning,
                        EntityType = "Offer",
                        EntityId = offer.Id,
                        UserId = 1, // System notification
                        IsRead = false
                    };

                    _context.Notifications.Add(notification);
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Sent {Count} offer reminders", offersNearExpiry.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending offer reminders");
                throw;
            }
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task CleanupOtpCodesAsync()
        {
            _logger.LogInformation("Starting OTP cleanup job");

            try
            {
                var cutoffTime = DateTime.UtcNow.AddMinutes(-10); // 10 minutes old
                var expiredOtpOffers = await _context.Offers
                    .Where(o => !string.IsNullOrEmpty(o.OtpCodeHash) &&
                               o.OtpSentAt.HasValue &&
                               o.OtpSentAt.Value < cutoffTime &&
                               !o.IsOtpVerified)
                    .ToListAsync();

                foreach (var offer in expiredOtpOffers)
                {
                    offer.OtpCodeHash = null;
                    offer.OtpSentAt = null;
                }

                await _context.SaveChangesAsync();

                _logger.LogInformation("Cleaned up {Count} expired OTP codes", expiredOtpOffers.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cleaning up OTP codes");
                throw;
            }
        }

        [AutomaticRetry(Attempts = 3)]
        public async Task GenerateDealerCommissionReportsAsync()
        {
            _logger.LogInformation("Starting dealer commission reports job");

            try
            {
                var lastMonth = DateTime.UtcNow.AddMonths(-1);
                var startOfMonth = new DateTime(lastMonth.Year, lastMonth.Month, 1);
                var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1);

                var commissions = await _context.DealerCommissions
                    .Include(dc => dc.Dealer)
                    .Where(dc => dc.CreatedAt >= startOfMonth && 
                                dc.CreatedAt <= endOfMonth &&
                                dc.Status == CommissionStatus.Approved)
                    .GroupBy(dc => dc.DealerId)
                    .Select(g => new
                    {
                        DealerId = g.Key,
                        DealerName = g.First().Dealer.CompanyName,
                        TotalCommission = g.Sum(x => x.CommissionAmount),
                        Count = g.Count()
                    })
                    .ToListAsync();

                // TODO: Generate actual report (PDF, Excel, etc.)
                _logger.LogInformation("Generated commission reports for {Count} dealers", commissions.Count);

                foreach (var commission in commissions)
                {
                    _logger.LogInformation("Dealer {DealerName}: {TotalCommission:C} ({Count} commissions)",
                        commission.DealerName, commission.TotalCommission, commission.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating dealer commission reports");
                throw;
            }
        }
    }
}