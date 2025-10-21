using Microsoft.AspNetCore.Mvc;
using Oduyo.Domain.DTOs;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackagesController : ControllerBase
    {
        private readonly IPackageService _packageService;

        public PackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePackageDto dto)
        {
            var package = await _packageService.CreatePackageAsync(dto);
            return Ok(package);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePackageDto dto)
        {
            var package = await _packageService.UpdatePackageAsync(id, dto);
            if (package == null)
                return NotFound();
            return Ok(package);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _packageService.DeletePackageAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var package = await _packageService.GetPackageByIdAsync(id);
            if (package == null)
                return NotFound();
            return Ok(package);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var packages = await _packageService.GetAllPackagesAsync();
            return Ok(packages);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var packages = await _packageService.GetActivePackagesAsync();
            return Ok(packages);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            var packages = await _packageService.GetPackagesByProductAsync(productId);
            return Ok(packages);
        }
    }
}
