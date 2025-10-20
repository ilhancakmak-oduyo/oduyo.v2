using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantSettingsController : ControllerBase
    {
        private readonly ITenantSettingService _tenantSettingService;

        public TenantSettingsController(ITenantSettingService tenantSettingService)
        {
            _tenantSettingService = tenantSettingService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTenantSettingDto dto)
        {
            var setting = await _tenantSettingService.CreateSettingAsync(dto);
            return Ok(setting);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTenantSettingDto dto)
        {
            var setting = await _tenantSettingService.UpdateSettingAsync(id, dto);
            if (setting == null)
                return NotFound();
            return Ok(setting);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tenantSettingService.DeleteSettingAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var setting = await _tenantSettingService.GetSettingByIdAsync(id);
            if (setting == null)
                return NotFound();
            return Ok(setting);
        }

        [HttpGet("tenant/{tenantId}")]
        public async Task<IActionResult> GetTenantSettings(int tenantId)
        {
            var settings = await _tenantSettingService.GetTenantSettingsAsync(tenantId);
            return Ok(settings);
        }

        [HttpGet("tenant/{tenantId}/key/{key}")]
        public async Task<IActionResult> GetTenantSettingByKey(int tenantId, string key)
        {
            var setting = await _tenantSettingService.GetTenantSettingByKeyAsync(tenantId, key);
            if (setting == null)
                return NotFound();
            return Ok(setting);
        }

        [HttpGet("tenant/{tenantId}/value/{key}")]
        public async Task<IActionResult> GetTenantSettingValue(int tenantId, string key, [FromQuery] string? defaultValue = null)
        {
            var value = await _tenantSettingService.GetTenantSettingValueAsync(tenantId, key, defaultValue);
            return Ok(new { Value = value });
        }
    }
}
