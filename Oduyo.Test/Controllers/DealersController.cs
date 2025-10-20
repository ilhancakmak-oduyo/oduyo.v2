using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DealersController : ControllerBase
    {
        private readonly IDealerService _dealerService;

        public DealersController(IDealerService dealerService)
        {
            _dealerService = dealerService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDealerDto dto)
        {
            var dealer = await _dealerService.CreateDealerAsync(dto);
            return Ok(dealer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDealerDto dto)
        {
            var dealer = await _dealerService.UpdateDealerAsync(id, dto);
            if (dealer == null)
                return NotFound();
            return Ok(dealer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dealerService.DeleteDealerAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var dealer = await _dealerService.GetDealerByIdAsync(id);
            if (dealer == null)
                return NotFound();
            return Ok(dealer);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var dealers = await _dealerService.GetAllDealersAsync();
            return Ok(dealers);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var dealers = await _dealerService.GetActiveDealersAsync();
            return Ok(dealers);
        }
    }
}
