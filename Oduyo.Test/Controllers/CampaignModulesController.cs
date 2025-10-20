using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CampaignModulesController : ControllerBase
    {
        private readonly ICampaignModuleService _campaignModuleService;

        public CampaignModulesController(ICampaignModuleService campaignModuleService)
        {
            _campaignModuleService = campaignModuleService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] CampaignModuleDto dto)
        {
            var result = await _campaignModuleService.AssignModuleToCampaignAsync(dto.CampaignId, dto.ModuleId);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] CampaignModuleDto dto)
        {
            var result = await _campaignModuleService.RemoveModuleFromCampaignAsync(dto.CampaignId, dto.ModuleId);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("campaign/{campaignId}/modules")]
        public async Task<IActionResult> GetCampaignModules(int campaignId)
        {
            var modules = await _campaignModuleService.GetCampaignModulesAsync(campaignId);
            return Ok(modules);
        }

        [HttpGet("is-in-campaign/{campaignId}/{moduleId}")]
        public async Task<IActionResult> IsModuleInCampaign(int campaignId, int moduleId)
        {
            var isInCampaign = await _campaignModuleService.IsModuleInCampaignAsync(campaignId, moduleId);
            return Ok(new { IsInCampaign = isInCampaign });
        }
    }

    public class CampaignModuleDto
    {
        public int CampaignId { get; set; }
        public int ModuleId { get; set; }
    }
}
