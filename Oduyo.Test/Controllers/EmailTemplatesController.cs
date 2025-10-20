using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailTemplatesController : ControllerBase
    {
        private readonly IEmailTemplateService _emailTemplateService;

        public EmailTemplatesController(IEmailTemplateService emailTemplateService)
        {
            _emailTemplateService = emailTemplateService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmailTemplateDto dto)
        {
            var template = await _emailTemplateService.CreateTemplateAsync(dto);
            return Ok(template);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmailTemplateDto dto)
        {
            var template = await _emailTemplateService.UpdateTemplateAsync(id, dto);
            if (template == null)
                return NotFound();
            return Ok(template);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _emailTemplateService.DeleteTemplateAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var template = await _emailTemplateService.GetTemplateByIdAsync(id);
            if (template == null)
                return NotFound();
            return Ok(template);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var templates = await _emailTemplateService.GetAllTemplatesAsync();
            return Ok(templates);
        }

        [HttpGet("key/{templateKey}")]
        public async Task<IActionResult> GetByKey(string templateKey)
        {
            var template = await _emailTemplateService.GetTemplateByKeyAsync(templateKey);
            if (template == null)
                return NotFound();
            return Ok(template);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var templates = await _emailTemplateService.GetActiveTemplatesAsync();
            return Ok(templates);
        }
    }
}
