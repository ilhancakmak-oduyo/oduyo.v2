using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Constants;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Messages;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class CreditUsageService : ICreditUsageService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBus _bus;
        private readonly ILogger<CreditUsageService> _logger;

        public CreditUsageService(
            ApplicationDbContext context,
            IBus bus,
            ILogger<CreditUsageService> logger)
        {
            _context = context;
            _bus = bus;
            _logger = logger;
        }

        public async Task<bool> ConsumeCreditsAsync(int companyId, int amount, string description)
        {
            // Get available credits (FIFO - oldest first)
            var credit = await _context.Credits
                .Where(c => c.CompanyId == companyId)
                .Where(c => c.ExpiryDate > DateTime.UtcNow)
                .Where(c => c.RemainingCredit > 0)
                .OrderBy(c => c.ExpiryDate)
                .FirstOrDefaultAsync();

            if (credit == null || credit.RemainingCredit < amount)
            {
                _logger.LogWarning(
                    "Insufficient credits for company {CompanyId}. Required: {Amount}, Available: {Available}",
                    companyId, amount, credit?.RemainingCredit ?? 0
                );
                return false;
            }

            // Consume credits
            credit.UsedCredit += amount;
            credit.RemainingCredit -= amount;

            // Log usage
            var usage = new CreditUsage
            {
                CreditId = credit.Id,
                CompanyId = companyId,
                Amount = amount,
                Description = description,
                UsedAt = DateTime.UtcNow
            };
            _context.CreditUsages.Add(usage);

            await _context.SaveChangesAsync();

            // Warn if low
            if (credit.RemainingCredit < 100)
            {
                var company = await _context.Companies.FindAsync(companyId);
                await _bus.Publish(new SendEmailMessage
                {
                    To = company.Email,
                    Subject = "Kredi Azaldı",
                    TemplateId = "credit-low",
                    TemplateData = new Dictionary<string, string>
                    {
                        ["RemainingCredit"] = credit.RemainingCredit.ToString()
                    }
                });
            }

            _logger.LogInformation(
                "Consumed {Amount} credits for company {CompanyId}. Remaining: {Remaining}",
                amount, companyId, credit.RemainingCredit
            );

            return true;
        }

        public async Task<int> GetRemainingCreditsAsync(int companyId)
        {
            return await _context.Credits
                .Where(c => c.CompanyId == companyId)
                .Where(c => c.ExpiryDate > DateTime.UtcNow)
                .SumAsync(c => c.RemainingCredit);
        }

        public async Task<bool> HasSufficientCreditsAsync(int companyId, int required)
        {
            var available = await GetRemainingCreditsAsync(companyId);
            return available >= required;
        }

        public async Task<List<CreditUsage>> GetCreditUsageHistoryAsync(int companyId, DateTime? from = null, DateTime? to = null)
        {
            var query = _context.CreditUsages
                .Include(cu => cu.Credit)
                .Where(cu => cu.CompanyId == companyId);

            if (from.HasValue)
                query = query.Where(cu => cu.UsedAt >= from.Value);

            if (to.HasValue)
                query = query.Where(cu => cu.UsedAt <= to.Value);

            return await query
                .OrderByDescending(cu => cu.UsedAt)
                .ToListAsync();
        }

        public async Task<CreditUsageSummary> GetCreditUsageSummaryAsync(int companyId)
        {
            var totalCredits = await _context.Credits
                .Where(c => c.CompanyId == companyId)
                .SumAsync(c => c.TotalCredit);

            var usedCredits = await _context.Credits
                .Where(c => c.CompanyId == companyId)
                .SumAsync(c => c.UsedCredit);

            var remainingCredits = await GetRemainingCreditsAsync(companyId);

            var expiredCredits = await _context.Credits
                .Where(c => c.CompanyId == companyId)
                .Where(c => c.ExpiryDate <= DateTime.UtcNow)
                .SumAsync(c => c.RemainingCredit);

            return new CreditUsageSummary
            {
                CompanyId = companyId,
                TotalCredits = totalCredits,
                UsedCredits = usedCredits,
                RemainingCredits = remainingCredits,
                ExpiredCredits = expiredCredits
            };
        }
    }

    public interface ICreditUsageService
    {
        Task<bool> ConsumeCreditsAsync(int companyId, int amount, string description);
        Task<int> GetRemainingCreditsAsync(int companyId);
        Task<bool> HasSufficientCreditsAsync(int companyId, int required);
        Task<List<CreditUsage>> GetCreditUsageHistoryAsync(int companyId, DateTime? from = null, DateTime? to = null);
        Task<CreditUsageSummary> GetCreditUsageSummaryAsync(int companyId);
    }

    public class CreditUsageSummary
    {
        public int CompanyId { get; set; }
        public int TotalCredits { get; set; }
        public int UsedCredits { get; set; }
        public int RemainingCredits { get; set; }
        public int ExpiredCredits { get; set; }
    }
}
