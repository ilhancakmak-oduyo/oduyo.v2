using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfferRevisionsController : ControllerBase
    {
        private readonly IOfferRevisionService _offerRevisionService;

        public OfferRevisionsController(IOfferRevisionService offerRevisionService)
        {
            _offerRevisionService = offerRevisionService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOfferRevisionDto dto)
        {
            var revision = await _offerRevisionService.CreateRevisionAsync(dto);
            return Ok(revision);
        }

        [HttpGet("offer/{offerId}")]
        public async Task<IActionResult> GetOfferRevisions(int offerId)
        {
            var revisions = await _offerRevisionService.GetOfferRevisionsAsync(offerId);
            return Ok(revisions);
        }

        [HttpGet("offer/{offerId}/count")]
        public async Task<IActionResult> GetRevisionCount(int offerId)
        {
            var count = await _offerRevisionService.GetRevisionCountAsync(offerId);
            return Ok(new { Count = count });
        }
    }
}
