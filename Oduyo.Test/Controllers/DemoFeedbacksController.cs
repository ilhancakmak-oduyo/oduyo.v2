using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DemoFeedbacksController : ControllerBase
    {
        private readonly IDemoFeedbackService _demoFeedbackService;

        public DemoFeedbacksController(IDemoFeedbackService demoFeedbackService)
        {
            _demoFeedbackService = demoFeedbackService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDemoFeedbackDto dto)
        {
            var feedback = await _demoFeedbackService.CreateFeedbackAsync(dto);
            return Ok(feedback);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDemoFeedbackDto dto)
        {
            var feedback = await _demoFeedbackService.UpdateFeedbackAsync(id, dto);
            if (feedback == null)
                return NotFound();
            return Ok(feedback);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _demoFeedbackService.DeleteFeedbackAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var feedback = await _demoFeedbackService.GetFeedbackByIdAsync(id);
            if (feedback == null)
                return NotFound();
            return Ok(feedback);
        }

        [HttpGet("demo/{demoId}")]
        public async Task<IActionResult> GetDemoFeedback(int demoId)
        {
            var feedback = await _demoFeedbackService.GetDemoFeedbackAsync(demoId);
            if (feedback == null)
                return NotFound();
            return Ok(feedback);
        }
    }
}
