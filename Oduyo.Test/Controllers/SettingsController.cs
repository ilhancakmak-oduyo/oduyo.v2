using Microsoft.AspNetCore.Mvc;
using Oduyo.Domain.DTOs;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSettingDto dto)
        {
            var setting = await _settingService.CreateSettingAsync(dto);
            return Ok(setting);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSettingDto dto)
        {
            var setting = await _settingService.UpdateSettingAsync(id, dto);
            if (setting == null)
                return NotFound();
            return Ok(setting);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _settingService.DeleteSettingAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var setting = await _settingService.GetSettingByIdAsync(id);
            if (setting == null)
                return NotFound();
            return Ok(setting);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var settings = await _settingService.GetAllSettingsAsync();
            return Ok(settings);
        }

        [HttpGet("key/{key}")]
        public async Task<IActionResult> GetByKey(string key)
        {
            var setting = await _settingService.GetSettingByKeyAsync(key);
            if (setting == null)
                return NotFound();
            return Ok(setting);
        }

        [HttpGet("value/{key}")]
        public async Task<IActionResult> GetValue(string key, [FromQuery] string? defaultValue = null)
        {
            var value = await _settingService.GetSettingValueAsync(key, defaultValue);
            return Ok(new { Value = value });
        }
    }
}
