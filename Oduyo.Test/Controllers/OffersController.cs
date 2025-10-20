using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Enums;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OffersController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OffersController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOfferDto dto)
        {
            var offer = await _offerService.CreateOfferAsync(dto);
            return Ok(offer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOfferDto dto)
        {
            var offer = await _offerService.UpdateOfferAsync(id, dto);
            if (offer == null)
                return NotFound();
            return Ok(offer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _offerService.DeleteOfferAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var offer = await _offerService.GetOfferByIdAsync(id);
            if (offer == null)
                return NotFound();
            return Ok(offer);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var offers = await _offerService.GetAllOffersAsync();
            return Ok(offers);
        }

        [HttpGet("offer-no/{offerNo}")]
        public async Task<IActionResult> GetByOfferNo(string offerNo)
        {
            var offer = await _offerService.GetOfferByOfferNoAsync(offerNo);
            if (offer == null)
                return NotFound();
            return Ok(offer);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var offers = await _offerService.GetOffersByCompanyAsync(companyId);
            return Ok(offers);
        }

        [HttpGet("dealer/{dealerId}")]
        public async Task<IActionResult> GetByDealer(int dealerId)
        {
            var offers = await _offerService.GetOffersByDealerAsync(dealerId);
            return Ok(offers);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(OfferStatus status)
        {
            var offers = await _offerService.GetOffersByStatusAsync(status);
            return Ok(offers);
        }

        [HttpGet("expired")]
        public async Task<IActionResult> GetExpired()
        {
            var offers = await _offerService.GetExpiredOffersAsync();
            return Ok(offers);
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await _offerService.ApproveOfferAsync(id);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> Reject(int id, [FromBody] RejectOfferDto dto)
        {
            var result = await _offerService.RejectOfferAsync(id, dto.RejectionReason);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpGet("generate-offer-no")]
        public async Task<IActionResult> GenerateOfferNo()
        {
            var offerNo = await _offerService.GenerateOfferNoAsync();
            return Ok(new { OfferNo = offerNo });
        }

        [HttpGet("{id}/calculate-total")]
        public async Task<IActionResult> CalculateTotal(int id)
        {
            var total = await _offerService.CalculateOfferTotalAsync(id);
            return Ok(new { Total = total });
        }
    }

    public class RejectOfferDto
    {
        public required string RejectionReason { get; set; }
    }
}
