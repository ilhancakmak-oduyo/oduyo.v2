using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfferApprovalsController : ControllerBase
    {
        private readonly IOfferApprovalService _offerApprovalService;

        public OfferApprovalsController(IOfferApprovalService offerApprovalService)
        {
            _offerApprovalService = offerApprovalService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateApprovalRequest([FromBody] CreateOfferApprovalDto dto)
        {
            var approval = await _offerApprovalService.CreateApprovalRequestAsync(dto);
            return Ok(approval);
        }

        [HttpPost("{id}/process")]
        public async Task<IActionResult> ProcessApproval(int id, [FromBody] ApproveOfferDto dto)
        {
            var approval = await _offerApprovalService.ProcessApprovalAsync(id, dto);
            if (approval == null)
                return NotFound();
            return Ok(approval);
        }

        [HttpGet("offer/{offerId}")]
        public async Task<IActionResult> GetOfferApprovals(int offerId)
        {
            var approvals = await _offerApprovalService.GetOfferApprovalsAsync(offerId);
            return Ok(approvals);
        }

        [HttpGet("user/{approverUserId}/pending")]
        public async Task<IActionResult> GetPendingApprovalsByUser(int approverUserId)
        {
            var approvals = await _offerApprovalService.GetPendingApprovalsByUserAsync(approverUserId);
            return Ok(approvals);
        }

        [HttpGet("offer/{offerId}/pending")]
        public async Task<IActionResult> GetPendingApproval(int offerId)
        {
            var approval = await _offerApprovalService.GetPendingApprovalAsync(offerId);
            if (approval == null)
                return NotFound();
            return Ok(approval);
        }
    }
}
