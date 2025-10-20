using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignPackagesController : ControllerBase
    {
        private readonly ICampaignPackageService _campaignPackageService;

        public CampaignPackagesController(ICampaignPackageService campaignPackageService)
        {
            _campaignPackageService = campaignPackageService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] CampaignPackageDto dto)
        {
            var result = await _campaignPackageService.AssignPackageToCampaignAsync(dto.CampaignId, dto.PackageId);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] CampaignPackageDto dto)
        {
            var result = await _campaignPackageService.RemovePackageFromCampaignAsync(dto.CampaignId, dto.PackageId);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("campaign/{campaignId}/packages")]
        public async Task<IActionResult> GetCampaignPackages(int campaignId)
        {
            var packages = await _campaignPackageService.GetCampaignPackagesAsync(campaignId);
            return Ok(packages);
        }

        [HttpGet("is-in-campaign/{campaignId}/{packageId}")]
        public async Task<IActionResult> IsPackageInCampaign(int campaignId, int packageId)
        {
            var isInCampaign = await _campaignPackageService.IsPackageInCampaignAsync(campaignId, packageId);
            return Ok(new { IsInCampaign = isInCampaign });
        }
    }

    public class CampaignPackageDto
    {
        public int CampaignId { get; set; }
        public int PackageId { get; set; }
    }
}
