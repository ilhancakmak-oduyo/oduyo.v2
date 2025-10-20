using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;
using Oduyo.Domain.Messages;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class DealerCommissionService : IDealerCommissionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IBus _bus;
        private readonly ILogger<DealerCommissionService> _logger;

        public DealerCommissionService(
            ApplicationDbContext context,
            IBus bus,
            ILogger<DealerCommissionService> logger)
        {
            _context = context;
            _bus = bus;
            _logger = logger;
        }

        public async Task<DealerCommission> CreateCommissionAsync(
            CreateDealerCommissionDto dto)
        {
            var commission = new DealerCommission
            {
                DealerId = dto.DealerId,
                CompanyId = dto.CompanyId,
                OfferId = dto.OfferId,
                PaymentId = dto.PaymentId,
                BaseAmount = dto.BaseAmount,
                CommissionRate = dto.CommissionRate,
                CommissionAmount = dto.BaseAmount * (dto.CommissionRate / 100),
                Status = CommissionStatus.Pending,
                Notes = dto.Notes
            };

            _context.DealerCommissions.Add(commission);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Commission created for dealer {DealerId}, amount {Amount}",
                dto.DealerId, commission.CommissionAmount
            );

            return commission;
        }

        public async Task<bool> ApproveCommissionAsync(int commissionId)
        {
            var commission = await _context.DealerCommissions
                .Include(c => c.Dealer)
                .FirstOrDefaultAsync(c => c.Id == commissionId);

            if (commission == null)
                return false;

            commission.Status = CommissionStatus.Approved;
            await _context.SaveChangesAsync();

            // Notify dealer
            await _bus.Publish(new SendEmailMessage
            {
                To = commission.Dealer.Email,
                Subject = "Hak Ediş Onaylandı",
                TemplateId = "commission-approved",
                TemplateData = new Dictionary<string, string>
                {
                    ["Amount"] = commission.CommissionAmount.ToString("N2")
                }
            });

            _logger.LogInformation(
                "Commission {CommissionId} approved for dealer {DealerId}",
                commissionId, commission.DealerId
            );

            return true;
        }

        public async Task<bool> PayCommissionAsync(int commissionId)
        {
            var commission = await _context.DealerCommissions
                .Include(c => c.Dealer)
                .FirstOrDefaultAsync(c => c.Id == commissionId);

            if (commission == null || commission.Status != CommissionStatus.Approved)
                return false;

            commission.Status = CommissionStatus.Paid;
            commission.PaidAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Notify dealer
            await _bus.Publish(new SendEmailMessage
            {
                To = commission.Dealer.Email,
                Subject = "Hak Ediş Ödendi",
                TemplateId = "commission-paid",
                TemplateData = new Dictionary<string, string>
                {
                    ["Amount"] = commission.CommissionAmount.ToString("N2"),
                    ["PaidDate"] = commission.PaidAt.Value.ToString("dd.MM.yyyy")
                }
            });

            _logger.LogInformation(
                "Commission {CommissionId} paid to dealer {DealerId}",
                commissionId, commission.DealerId
            );

            return true;
        }

        public async Task<bool> CancelCommissionAsync(int commissionId, string reason)
        {
            var commission = await _context.DealerCommissions.FindAsync(commissionId);

            if (commission == null || commission.Status == CommissionStatus.Paid)
                return false;

            commission.Status = CommissionStatus.Cancelled;
            commission.Notes = $"{commission.Notes}\n[İptal Nedeni: {reason}]";
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Commission {CommissionId} cancelled. Reason: {Reason}",
                commissionId, reason
            );

            return true;
        }

        public async Task<DealerCommissionReport> GetDealerReportAsync(int dealerId, DateTime? from = null)
        {
            var query = _context.DealerCommissions.Where(dc => dc.DealerId == dealerId);

            if (from.HasValue)
                query = query.Where(dc => dc.CreatedAt >= from.Value);

            var commissions = await query
                .Include(dc => dc.Company)
                .Include(dc => dc.Offer)
                .Include(dc => dc.Payment)
                .ToListAsync();

            return new DealerCommissionReport
            {
                DealerId = dealerId,
                TotalCommissionAmount = commissions.Sum(c => c.CommissionAmount),
                PendingAmount = commissions
                    .Where(c => c.Status == CommissionStatus.Pending)
                    .Sum(c => c.CommissionAmount),
                ApprovedAmount = commissions
                    .Where(c => c.Status == CommissionStatus.Approved)
                    .Sum(c => c.CommissionAmount),
                PaidAmount = commissions
                    .Where(c => c.Status == CommissionStatus.Paid)
                    .Sum(c => c.CommissionAmount),
                CommissionCount = commissions.Count,
                Commissions = commissions
            };
        }

        public async Task<List<DealerCommission>> GetCommissionsByCompanyAsync(int companyId)
        {
            return await _context.DealerCommissions
                .Include(dc => dc.Dealer)
                .Where(dc => dc.CompanyId == companyId)
                .OrderByDescending(dc => dc.CreatedAt)
                .ToListAsync();
        }

        public async Task<DealerCommission> GetCommissionByIdAsync(int commissionId)
        {
            return await _context.DealerCommissions
                .Include(dc => dc.Dealer)
                .Include(dc => dc.Company)
                .Include(dc => dc.Offer)
                .Include(dc => dc.Payment)
                .FirstOrDefaultAsync(dc => dc.Id == commissionId);
        }
    }

    public interface IDealerCommissionService
    {
        Task<DealerCommission> CreateCommissionAsync(CreateDealerCommissionDto dto);
        Task<bool> ApproveCommissionAsync(int commissionId);
        Task<bool> PayCommissionAsync(int commissionId);
        Task<bool> CancelCommissionAsync(int commissionId, string reason);
        Task<DealerCommissionReport> GetDealerReportAsync(int dealerId, DateTime? from = null);
        Task<List<DealerCommission>> GetCommissionsByCompanyAsync(int companyId);
        Task<DealerCommission> GetCommissionByIdAsync(int commissionId);
    }

    public class CreateDealerCommissionDto
    {
        public int DealerId { get; set; }
        public int CompanyId { get; set; }
        public int? OfferId { get; set; }
        public int? PaymentId { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal CommissionRate { get; set; }
        public string Notes { get; set; }
    }

    public class DealerCommissionReport
    {
        public int DealerId { get; set; }
        public decimal TotalCommissionAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public decimal ApprovedAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public int CommissionCount { get; set; }
        public List<DealerCommission> Commissions { get; set; }
    }
}
