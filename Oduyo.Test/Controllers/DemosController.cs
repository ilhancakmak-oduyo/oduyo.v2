using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Enums;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemosController : ControllerBase
    {
        private readonly IDemoService _demoService;

        public DemosController(IDemoService demoService)
        {
            _demoService = demoService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDemoDto dto)
        {
            var demo = await _demoService.CreateDemoAsync(dto);
            return Ok(demo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDemoDto dto)
        {
            var demo = await _demoService.UpdateDemoAsync(id, dto);
            if (demo == null)
                return NotFound();
            return Ok(demo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _demoService.DeleteDemoAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var demo = await _demoService.GetDemoByIdAsync(id);
            if (demo == null)
                return NotFound();
            return Ok(demo);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var demos = await _demoService.GetAllDemosAsync();
            return Ok(demos);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var demos = await _demoService.GetDemosByCompanyAsync(companyId);
            return Ok(demos);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(DemoStatus status)
        {
            var demos = await _demoService.GetDemosByStatusAsync(status);
            return Ok(demos);
        }

        [HttpGet("assigned-user/{assignedUserId}")]
        public async Task<IActionResult> GetByAssignedUser(int assignedUserId)
        {
            var demos = await _demoService.GetDemosByAssignedUserAsync(assignedUserId);
            return Ok(demos);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcoming()
        {
            var demos = await _demoService.GetUpcomingDemosAsync();
            return Ok(demos);
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(int id)
        {
            var result = await _demoService.CompleteDemoAsync(id);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _demoService.CancelDemoAsync(id);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("{id}/postpone")]
        public async Task<IActionResult> Postpone(int id, [FromBody] PostponeDemoDto dto)
        {
            var result = await _demoService.PostponeDemoAsync(id, dto.NewDate);
            if (!result)
                return BadRequest();
            return Ok(result);
        }
    }

    public class PostponeDemoDto
    {
        public DateTime NewDate { get; set; }
    }
}
