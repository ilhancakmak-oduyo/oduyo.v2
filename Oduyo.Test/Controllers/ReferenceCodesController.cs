using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReferenceCodesController : ControllerBase
    {
        private readonly IReferenceCodeService _referenceCodeService;

        public ReferenceCodesController(IReferenceCodeService referenceCodeService)
        {
            _referenceCodeService = referenceCodeService;
        }

        [HttpGet("generate/{prefix}")]
        public async Task<IActionResult> GenerateReferenceCode(string prefix)
        {
            var referenceCode = await _referenceCodeService.GenerateReferenceCodeAsync(prefix);
            return Ok(new { ReferenceCode = referenceCode });
        }

        [HttpGet("sequence/{prefix}")]
        public async Task<IActionResult> GetSequence(string prefix)
        {
            var sequence = await _referenceCodeService.GetSequenceAsync(prefix);
            if (sequence == null)
                return NotFound();
            return Ok(sequence);
        }
    }
}
