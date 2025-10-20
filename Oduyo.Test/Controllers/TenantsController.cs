using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantService _tenantService;

        public TenantsController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTenantDto dto)
        {
            var tenant = await _tenantService.CreateTenantAsync(dto);
            return Ok(tenant);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTenantDto dto)
        {
            var tenant = await _tenantService.UpdateTenantAsync(id, dto);
            if (tenant == null)
                return NotFound();
            return Ok(tenant);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _tenantService.DeleteTenantAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (tenant == null)
                return NotFound();
            return Ok(tenant);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tenants = await _tenantService.GetAllTenantsAsync();
            return Ok(tenants);
        }

        [HttpGet("domain/{domain}")]
        public async Task<IActionResult> GetByDomain(string domain)
        {
            var tenant = await _tenantService.GetTenantByDomainAsync(domain);
            if (tenant == null)
                return NotFound();
            return Ok(tenant);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var tenants = await _tenantService.GetActiveTenantsAsync();
            return Ok(tenants);
        }
    }
}
