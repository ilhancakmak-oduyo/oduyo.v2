using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DealerUserMappingsController : ControllerBase
    {
        private readonly IDealerUserMappingService _dealerUserMappingService;

        public DealerUserMappingsController(IDealerUserMappingService dealerUserMappingService)
        {
            _dealerUserMappingService = dealerUserMappingService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] DealerUserDto dto)
        {
            var result = await _dealerUserMappingService.AssignDealerToUserAsync(dto.DealerId, dto.UserId);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] DealerUserDto dto)
        {
            var result = await _dealerUserMappingService.RemoveDealerFromUserAsync(dto.DealerId, dto.UserId);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("user/{userId}/dealers")]
        public async Task<IActionResult> GetUserDealers(int userId)
        {
            var dealers = await _dealerUserMappingService.GetUserDealersAsync(userId);
            return Ok(dealers);
        }

        [HttpGet("dealer/{dealerId}/users")]
        public async Task<IActionResult> GetDealerUsers(int dealerId)
        {
            var users = await _dealerUserMappingService.GetDealerUsersAsync(dealerId);
            return Ok(users);
        }

        [HttpGet("is-assigned/{dealerId}/{userId}")]
        public async Task<IActionResult> IsUserAssignedToDealer(int dealerId, int userId)
        {
            var isAssigned = await _dealerUserMappingService.IsUserAssignedToDealerAsync(dealerId, userId);
            return Ok(new { IsAssigned = isAssigned });
        }
    }

    public class DealerUserDto
    {
        public int DealerId { get; set; }
        public int UserId { get; set; }
    }
}
