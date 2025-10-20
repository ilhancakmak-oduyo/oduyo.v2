using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class OfferApprovalService : IOfferApprovalService
    {
        private readonly ApplicationDbContext _context;

        public OfferApprovalService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OfferApproval> CreateApprovalRequestAsync(CreateOfferApprovalDto dto)
        {
            var approval = new OfferApproval
            {
                OfferId = dto.OfferId,
                RequestedUserId = dto.RequestedUserId,
                ApproverUserId = dto.ApproverUserId,
                IsApproved = false,
                Notes = dto.Notes
            };

            _context.OfferApprovals.Add(approval);
            await _context.SaveChangesAsync();
            return approval;
        }

        public async Task<OfferApproval> ProcessApprovalAsync(int approvalId, ApproveOfferDto dto)
        {
            var approval = await _context.OfferApprovals.FindAsync(approvalId);
            if (approval == null)
                throw new InvalidOperationException("Onay kaydı bulunamadı.");

            approval.IsApproved = dto.IsApproved;
            approval.Notes = dto.Notes;
            approval.ApprovedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return approval;
        }

        public async Task<List<OfferApproval>> GetOfferApprovalsAsync(int offerId)
        {
            return await _context.OfferApprovals
                .Where(oa => oa.OfferId == offerId)
                .OrderByDescending(oa => oa.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<OfferApproval>> GetPendingApprovalsByUserAsync(int approverUserId)
        {
            return await _context.OfferApprovals
                .Where(oa => oa.ApproverUserId == approverUserId && oa.ApprovedAt == null)
                .OrderBy(oa => oa.CreatedAt)
                .ToListAsync();
        }

        public async Task<OfferApproval> GetPendingApprovalAsync(int offerId)
        {
            return await _context.OfferApprovals
                .Where(oa => oa.OfferId == offerId && oa.ApprovedAt == null)
                .FirstOrDefaultAsync();
        }
    }
}