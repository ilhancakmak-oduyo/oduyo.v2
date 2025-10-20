using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackageModulesController : ControllerBase
    {
        private readonly IPackageModuleService _packageModuleService;

        public PackageModulesController(IPackageModuleService packageModuleService)
        {
            _packageModuleService = packageModuleService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] AssignPackageModuleDto dto)
        {
            var result = await _packageModuleService.AssignModuleToPackageAsync(dto.PackageId, dto.ModuleId, dto.IsFree);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] PackageModuleDto dto)
        {
            var result = await _packageModuleService.RemoveModuleFromPackageAsync(dto.PackageId, dto.ModuleId);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("package/{packageId}/modules")]
        public async Task<IActionResult> GetPackageModules(int packageId)
        {
            var modules = await _packageModuleService.GetPackageModulesAsync(packageId);
            return Ok(modules);
        }

        [HttpGet("package/{packageId}/free-modules")]
        public async Task<IActionResult> GetPackageFreeModules(int packageId)
        {
            var modules = await _packageModuleService.GetPackageFreeModulesAsync(packageId);
            return Ok(modules);
        }

        [HttpGet("is-in-package/{packageId}/{moduleId}")]
        public async Task<IActionResult> IsModuleInPackage(int packageId, int moduleId)
        {
            var isInPackage = await _packageModuleService.IsModuleInPackageAsync(packageId, moduleId);
            return Ok(new { IsInPackage = isInPackage });
        }
    }

    public class PackageModuleDto
    {
        public int PackageId { get; set; }
        public int ModuleId { get; set; }
    }

    public class AssignPackageModuleDto
    {
        public int PackageId { get; set; }
        public int ModuleId { get; set; }
        public bool IsFree { get; set; }
    }
}
