using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantUsersController : ControllerBase
    {
        private readonly ITenantUserService _tenantUserService;

        public TenantUsersController(ITenantUserService tenantUserService)
        {
            _tenantUserService = tenantUserService;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] TenantUserDto dto)
        {
            var result = await _tenantUserService.AssignTenantToUserAsync(dto.TenantId, dto.UserId);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> Remove([FromBody] TenantUserDto dto)
        {
            var result = await _tenantUserService.RemoveTenantFromUserAsync(dto.TenantId, dto.UserId);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("user/{userId}/tenants")]
        public async Task<IActionResult> GetUserTenants(int userId)
        {
            var tenants = await _tenantUserService.GetUserTenantsAsync(userId);
            return Ok(tenants);
        }

        [HttpGet("tenant/{tenantId}/users")]
        public async Task<IActionResult> GetTenantUsers(int tenantId)
        {
            var users = await _tenantUserService.GetTenantUsersAsync(tenantId);
            return Ok(users);
        }

        [HttpGet("is-assigned/{tenantId}/{userId}")]
        public async Task<IActionResult> IsUserAssignedToTenant(int tenantId, int userId)
        {
            var isAssigned = await _tenantUserService.IsUserAssignedToTenantAsync(tenantId, userId);
            return Ok(new { IsAssigned = isAssigned });
        }
    }

    public class TenantUserDto
    {
        public int TenantId { get; set; }
        public int UserId { get; set; }
    }
}
