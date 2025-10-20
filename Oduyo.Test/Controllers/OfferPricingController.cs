using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfferPricingController : ControllerBase
    {
        private readonly IOfferPricingService _offerPricingService;

        public OfferPricingController(IOfferPricingService offerPricingService)
        {
            _offerPricingService = offerPricingService;
        }

        [HttpPost("calculate")]
        public async Task<IActionResult> CalculatePricing([FromBody] OfferPricingRequest request)
        {
            var calculation = await _offerPricingService.CalculatePricingAsync(request);
            return Ok(calculation);
        }

        [HttpPost("validate-campaign")]
        public async Task<IActionResult> ValidateCampaignEligibility([FromBody] ValidateCampaignDto dto)
        {
            var isValid = await _offerPricingService.ValidateCampaignEligibilityAsync(dto.CampaignId, dto.PackageIds);
            return Ok(new { IsValid = isValid });
        }

        [HttpPost("calculate-crosssell-perks")]
        public async Task<IActionResult> CalculateCrossSellPerks([FromBody] List<OfferDetailDto> details)
        {
            var perks = await _offerPricingService.CalculateCrossSellPerksAsync(details);
            return Ok(new { CrossSellPerks = perks });
        }
    }

    public class ValidateCampaignDto
    {
        public int CampaignId { get; set; }
        public required IEnumerable<int> PackageIds { get; set; }
    }
}
