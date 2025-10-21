using Microsoft.AspNetCore.Mvc;
using Oduyo.Domain.DTOs;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        public PermissionsController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionDto dto)
        {
            var permission = await _permissionService.CreatePermissionAsync(dto);
            return Ok(permission);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePermissionDto dto)
        {
            var permission = await _permissionService.UpdatePermissionAsync(id, dto);
            if (permission == null)
                return NotFound();
            return Ok(permission);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _permissionService.DeletePermissionAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var permission = await _permissionService.GetPermissionByIdAsync(id);
            if (permission == null)
                return NotFound();
            return Ok(permission);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var permissions = await _permissionService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        [HttpGet("module/{module}")]
        public async Task<IActionResult> GetByModule(string module)
        {
            var permissions = await _permissionService.GetPermissionsByModuleAsync(module);
            return Ok(permissions);
        }

        [HttpPost("assign-to-role")]
        public async Task<IActionResult> AssignToRole([FromBody] PermissionRoleDto dto)
        {
            var result = await _permissionService.AssignPermissionToRoleAsync(dto.RoleId, dto.PermissionId);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("remove-from-role")]
        public async Task<IActionResult> RemoveFromRole([FromBody] PermissionRoleDto dto)
        {
            var result = await _permissionService.RemovePermissionFromRoleAsync(dto.RoleId, dto.PermissionId);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("role/{roleId}")]
        public async Task<IActionResult> GetRolePermissions(int roleId)
        {
            var permissions = await _permissionService.GetRolePermissionsAsync(roleId);
            return Ok(permissions);
        }

        [HttpGet("user/{userId}/has/{permissionCode}")]
        public async Task<IActionResult> UserHasPermission(int userId, string permissionCode)
        {
            var hasPermission = await _permissionService.UserHasPermissionAsync(userId, permissionCode);
            return Ok(new { HasPermission = hasPermission });
        }
    }

    public class PermissionRoleDto
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
