using Microsoft.AspNetCore.Mvc;
using Oduyo.Domain.DTOs;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LicensesController : ControllerBase
    {
        private readonly ILicenseService _licenseService;

        public LicensesController(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLicenseDto dto)
        {
            var license = await _licenseService.CreateLicenseAsync(dto);
            return Ok(license);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLicenseDto dto)
        {
            var license = await _licenseService.UpdateLicenseAsync(id, dto);
            if (license == null)
                return NotFound();
            return Ok(license);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _licenseService.DeleteLicenseAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var license = await _licenseService.GetLicenseByIdAsync(id);
            if (license == null)
                return NotFound();
            return Ok(license);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetCompanyLicenses(int companyId)
        {
            var licenses = await _licenseService.GetCompanyLicensesAsync(companyId);
            return Ok(licenses);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var licenses = await _licenseService.GetActiveLicensesAsync();
            return Ok(licenses);
        }

        [HttpGet("expiring/{daysBeforeExpiry}")]
        public async Task<IActionResult> GetExpiringLicenses(int daysBeforeExpiry)
        {
            var licenses = await _licenseService.GetExpiringLicensesAsync(daysBeforeExpiry);
            return Ok(licenses);
        }

        [HttpPost("{id}/renew")]
        public async Task<IActionResult> Renew(int id, [FromBody] RenewLicenseDto dto)
        {
            var result = await _licenseService.RenewLicenseAsync(id, dto.NewEndDate);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpGet("company/{companyId}/is-valid")]
        public async Task<IActionResult> IsLicenseValid(int companyId, [FromQuery] int? productId = null)
        {
            var isValid = await _licenseService.IsLicenseValidAsync(companyId, productId);
            return Ok(new { IsValid = isValid });
        }
    }

    public class RenewLicenseDto
    {
        public DateTime NewEndDate { get; set; }
    }
}
