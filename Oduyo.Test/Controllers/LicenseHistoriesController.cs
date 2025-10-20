using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicenseHistoriesController : ControllerBase
    {
        private readonly ILicenseHistoryService _licenseHistoryService;

        public LicenseHistoriesController(ILicenseHistoryService licenseHistoryService)
        {
            _licenseHistoryService = licenseHistoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLicenseHistoryDto dto)
        {
            var history = await _licenseHistoryService.CreateHistoryAsync(dto.LicenseId, dto.Action);
            return Ok(history);
        }

        [HttpGet("license/{licenseId}")]
        public async Task<IActionResult> GetLicenseHistory(int licenseId)
        {
            var history = await _licenseHistoryService.GetLicenseHistoryAsync(licenseId);
            return Ok(history);
        }
    }

    public class CreateLicenseHistoryDto
    {
        public int LicenseId { get; set; }
        public required string Action { get; set; }
    }
}
