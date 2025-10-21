using Microsoft.AspNetCore.Mvc;
using Oduyo.Domain.DTOs;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyContactsController : ControllerBase
    {
        private readonly ICompanyContactService _companyContactService;

        public CompanyContactsController(ICompanyContactService companyContactService)
        {
            _companyContactService = companyContactService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCompanyContactDto dto)
        {
            var contact = await _companyContactService.CreateContactAsync(dto);
            return Ok(contact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCompanyContactDto dto)
        {
            var contact = await _companyContactService.UpdateContactAsync(id, dto);
            if (contact == null)
                return NotFound();
            return Ok(contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _companyContactService.DeleteContactAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var contact = await _companyContactService.GetContactByIdAsync(id);
            if (contact == null)
                return NotFound();
            return Ok(contact);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetCompanyContacts(int companyId)
        {
            var contacts = await _companyContactService.GetCompanyContactsAsync(companyId);
            return Ok(contacts);
        }

        [HttpGet("company/{companyId}/primary")]
        public async Task<IActionResult> GetPrimaryContact(int companyId)
        {
            var contact = await _companyContactService.GetPrimaryContactAsync(companyId);
            if (contact == null)
                return NotFound();
            return Ok(contact);
        }

        [HttpPost("company/{companyId}/set-primary/{contactId}")]
        public async Task<IActionResult> SetPrimaryContact(int companyId, int contactId)
        {
            var result = await _companyContactService.SetPrimaryContactAsync(companyId, contactId);
            if (!result)
                return BadRequest();
            return Ok(result);
        }
    }
}
