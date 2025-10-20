using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Enums;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            var result = await _roleService.CreateRoleAsync(dto);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleDto dto)
        {
            var result = await _roleService.UpdateRoleAsync(id, dto);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roleService.DeleteRoleAsync(id);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();
            return Ok(role);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("type/{roleType}")]
        public async Task<IActionResult> GetByType(RoleType roleType)
        {
            var roles = await _roleService.GetRolesByTypeAsync(roleType);
            return Ok(roles);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleDto dto)
        {
            var result = await _roleService.AssignRoleToUserAsync(dto.UserId, dto.RoleId);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] AssignRoleDto dto)
        {
            var result = await _roleService.RemoveRoleFromUserAsync(dto.UserId, dto.RoleId);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var roles = await _roleService.GetUserRolesAsync(userId);
            return Ok(roles);
        }
    }

    public class AssignRoleDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
