using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public PermissionService(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Permission> CreatePermissionAsync(CreatePermissionDto dto)
        {
            var permission = new Permission
            {
                Name = dto.Name,
                Code = dto.Code,
                Module = dto.Module,
                Description = dto.Description,
                IsActive = true
            };

            _context.Permissions.Add(permission);
            await _context.SaveChangesAsync();
            return permission;
        }

        public async Task<Permission> UpdatePermissionAsync(int permissionId, UpdatePermissionDto dto)
        {
            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission == null) return null;

            permission.Name = dto.Name;
            permission.Description = dto.Description;
            permission.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return permission;
        }

        public async Task<bool> DeletePermissionAsync(int permissionId)
        {
            var permission = await _context.Permissions.FindAsync(permissionId);
            if (permission == null) return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Permission> GetPermissionByIdAsync(int permissionId)
        {
            return await _context.Permissions
                .Where(p => p.Id == permissionId && p.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions
                .Where(p => p.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<List<Permission>> GetPermissionsByModuleAsync(string module)
        {
            return await _context.Permissions
                .Where(p => p.Module == module && p.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<bool> AssignPermissionToRoleAsync(int roleId, int permissionId)
        {
            var exists = await _context.RolePermissions
                .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId && rp.DeletedAt == null);

            if (exists) return false;

            var rolePermission = new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            };

            _context.RolePermissions.Add(rolePermission);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePermissionFromRoleAsync(int roleId, int permissionId)
        {
            var rolePermission = await _context.RolePermissions
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId && rp.DeletedAt == null);

            if (rolePermission == null) return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Permission>> GetRolePermissionsAsync(int roleId)
        {
            return await _context.RolePermissions
                .Where(rp => rp.RoleId == roleId && rp.DeletedAt == null)
                .Join(_context.Permissions, rp => rp.PermissionId, p => p.Id, (rp, p) => p)
                .Where(p => p.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<bool> UserHasPermissionAsync(int userId, string permissionCode)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            var roleNames = await _userManager.GetRolesAsync(user);
            var roleIds = await _context.Roles
                .Where(r => roleNames.Contains(r.Name) && r.DeletedAt == null)
                .Select(r => r.Id)
                .ToListAsync();

            return await _context.RolePermissions
                .Where(rp => roleIds.Contains(rp.RoleId) && rp.DeletedAt == null)
                .Join(_context.Permissions, rp => rp.PermissionId, p => p.Id, (rp, p) => p)
                .AnyAsync(p => p.Code == permissionCode && p.DeletedAt == null);
        }
    }
}