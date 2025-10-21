using Microsoft.AspNetCore.Mvc;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Enums;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyDto dto)
        {
            var company = await _companyService.CreateCompanyAsync(dto);
            return Ok(company);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyDto dto)
        {
            var company = await _companyService.UpdateCompanyAsync(id, dto);
            if (company == null)
                return NotFound();
            return Ok(company);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _companyService.DeleteCompanyAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
                return NotFound();
            return Ok(company);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var companies = await _companyService.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("reference-code/{referenceCode}")]
        public async Task<IActionResult> GetByReferenceCode(string referenceCode)
        {
            var company = await _companyService.GetCompanyByReferenceCodeAsync(referenceCode);
            if (company == null)
                return NotFound();
            return Ok(company);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(CompanyStatus status)
        {
            var companies = await _companyService.GetCompaniesByStatusAsync(status);
            return Ok(companies);
        }

        [HttpGet("channel/{channelId}")]
        public async Task<IActionResult> GetByChannel(int channelId)
        {
            var companies = await _companyService.GetCompaniesByChannelAsync(channelId);
            return Ok(companies);
        }

        [HttpGet("dealer/{dealerId}")]
        public async Task<IActionResult> GetByDealer(int dealerId)
        {
            var companies = await _companyService.GetCompaniesByDealerAsync(dealerId);
            return Ok(companies);
        }

        [HttpGet("support-user/{supportUserId}")]
        public async Task<IActionResult> GetBySupportUser(int supportUserId)
        {
            var companies = await _companyService.GetCompaniesBySupportUserAsync(supportUserId);
            return Ok(companies);
        }

        [HttpGet("generate-reference-code")]
        public async Task<IActionResult> GenerateReferenceCode()
        {
            var referenceCode = await _companyService.GenerateReferenceCodeAsync();
            return Ok(new { ReferenceCode = referenceCode });
        }
    }
}
