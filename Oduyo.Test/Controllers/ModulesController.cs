using Microsoft.AspNetCore.Mvc;
using Oduyo.Domain.DTOs;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModulesController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateModuleDto dto)
        {
            var module = await _moduleService.CreateModuleAsync(dto);
            return Ok(module);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateModuleDto dto)
        {
            var module = await _moduleService.UpdateModuleAsync(id, dto);
            if (module == null)
                return NotFound();
            return Ok(module);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _moduleService.DeleteModuleAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var module = await _moduleService.GetModuleByIdAsync(id);
            if (module == null)
                return NotFound();
            return Ok(module);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var modules = await _moduleService.GetAllModulesAsync();
            return Ok(modules);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var modules = await _moduleService.GetActiveModulesAsync();
            return Ok(modules);
        }
    }
}
