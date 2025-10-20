using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfferDetailsController : ControllerBase
    {
        private readonly IOfferDetailService _offerDetailService;

        public OfferDetailsController(IOfferDetailService offerDetailService)
        {
            _offerDetailService = offerDetailService;
        }

        [HttpPost("offer/{offerId}")]
        public async Task<IActionResult> Add(int offerId, [FromBody] CreateOfferDetailDto dto)
        {
            var detail = await _offerDetailService.AddDetailAsync(offerId, dto);
            return Ok(detail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateOfferDetailDto dto)
        {
            var detail = await _offerDetailService.UpdateDetailAsync(id, dto);
            if (detail == null)
                return NotFound();
            return Ok(detail);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _offerDetailService.DeleteDetailAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var detail = await _offerDetailService.GetDetailByIdAsync(id);
            if (detail == null)
                return NotFound();
            return Ok(detail);
        }

        [HttpGet("offer/{offerId}")]
        public async Task<IActionResult> GetOfferDetails(int offerId)
        {
            var details = await _offerDetailService.GetOfferDetailsAsync(offerId);
            return Ok(details);
        }
    }
}
