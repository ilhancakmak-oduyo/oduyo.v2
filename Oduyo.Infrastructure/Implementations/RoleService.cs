using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public RoleService(RoleManager<Role> roleManager, UserManager<User> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> CreateRoleAsync(CreateRoleDto dto)
        {
            var role = new Role
            {
                Name = dto.Name,
                Description = dto.Description,
                RoleType = dto.RoleType,
                IsActive = true
            };

            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> UpdateRoleAsync(int roleId, UpdateRoleDto dto)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) return IdentityResult.Failed(new IdentityError { Description = "Rol bulunamadı" });

            role.Name = dto.Name;
            role.Description = dto.Description;
            role.IsActive = dto.IsActive;

            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(int roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) return IdentityResult.Failed(new IdentityError { Description = "Rol bulunamadı" });

            return await _roleManager.DeleteAsync(role);
        }

        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            return await _roleManager.FindByIdAsync(roleId.ToString());
        }

        public async Task<List<Role>> GetRolesByTypeAsync(RoleType roleType)
        {
            return await _context.Roles
                .Where(r => r.RoleType == roleType && r.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<List<Role>> GetAllRolesAsync()
        {
            return await _context.Roles
                .Where(r => r.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<IdentityResult> AssignRoleToUserAsync(int userId, int roleId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (user == null || role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı veya rol bulunamadı" });

            return await _userManager.AddToRoleAsync(user, role.Name);
        }

        public async Task<IdentityResult> RemoveRoleFromUserAsync(int userId, int roleId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var role = await _roleManager.FindByIdAsync(roleId.ToString());

            if (user == null || role == null)
                return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı veya rol bulunamadı" });

            return await _userManager.RemoveFromRoleAsync(user, role.Name);
        }

        public async Task<List<Role>> GetUserRolesAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new List<Role>();

            var roleNames = await _userManager.GetRolesAsync(user);
            return await _context.Roles.Where(r => roleNames.Contains(r.Name)).ToListAsync();
        }
    }
}