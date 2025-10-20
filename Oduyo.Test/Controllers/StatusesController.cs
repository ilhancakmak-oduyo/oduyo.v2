using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusesController : ControllerBase
    {
        private readonly IStatusService _statusService;

        public StatusesController(IStatusService statusService)
        {
            _statusService = statusService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStatusDto dto)
        {
            var status = await _statusService.CreateStatusAsync(dto);
            return Ok(status);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStatusDto dto)
        {
            var status = await _statusService.UpdateStatusAsync(id, dto);
            if (status == null)
                return NotFound();
            return Ok(status);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _statusService.DeleteStatusAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var status = await _statusService.GetStatusByIdAsync(id);
            if (status == null)
                return NotFound();
            return Ok(status);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var statuses = await _statusService.GetAllStatusesAsync();
            return Ok(statuses);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var statuses = await _statusService.GetActiveStatusesAsync();
            return Ok(statuses);
        }

        [HttpGet("entity-type/{entityType}")]
        public async Task<IActionResult> GetByEntityType(string entityType)
        {
            var statuses = await _statusService.GetStatusesByEntityTypeAsync(entityType);
            return Ok(statuses);
        }
    }
}
