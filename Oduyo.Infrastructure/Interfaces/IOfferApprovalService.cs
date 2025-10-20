using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IOfferApprovalService
    {
        Task<OfferApproval> CreateApprovalRequestAsync(CreateOfferApprovalDto dto);
        Task<OfferApproval> ProcessApprovalAsync(int approvalId, ApproveOfferDto dto);
        Task<List<OfferApproval>> GetOfferApprovalsAsync(int offerId);
        Task<List<OfferApproval>> GetPendingApprovalsByUserAsync(int approverUserId);
        Task<OfferApproval> GetPendingApprovalAsync(int offerId);
    }
}