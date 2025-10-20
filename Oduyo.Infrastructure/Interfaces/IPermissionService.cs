using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IPermissionService
    {
        Task<Permission> CreatePermissionAsync(CreatePermissionDto dto);
        Task<Permission> UpdatePermissionAsync(int permissionId, UpdatePermissionDto dto);
        Task<bool> DeletePermissionAsync(int permissionId);
        Task<Permission> GetPermissionByIdAsync(int permissionId);
        Task<List<Permission>> GetAllPermissionsAsync();
        Task<List<Permission>> GetPermissionsByModuleAsync(string module);
        Task<bool> AssignPermissionToRoleAsync(int roleId, int permissionId);
        Task<bool> RemovePermissionFromRoleAsync(int roleId, int permissionId);
        Task<List<Permission>> GetRolePermissionsAsync(int roleId);
        Task<bool> UserHasPermissionAsync(int userId, string permissionCode);
    }
}