using Microsoft.AspNetCore.Identity;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IRoleService
    {
        Task<IdentityResult> CreateRoleAsync(CreateRoleDto dto);
        Task<IdentityResult> UpdateRoleAsync(int roleId, UpdateRoleDto dto);
        Task<IdentityResult> DeleteRoleAsync(int roleId);
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<List<Role>> GetRolesByTypeAsync(RoleType roleType);
        Task<List<Role>> GetAllRolesAsync();
        Task<IdentityResult> AssignRoleToUserAsync(int userId, int roleId);
        Task<IdentityResult> RemoveRoleFromUserAsync(int userId, int roleId);
        Task<List<Role>> GetUserRolesAsync(int userId);
    }
}