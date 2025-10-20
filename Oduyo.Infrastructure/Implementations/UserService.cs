using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public UserService(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> CreateUserAsync(CreateUserDto dto)
        {
            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email,
                PhoneNumber = dto.Phone,
                UserType = dto.UserType,
                IsActive = true
            };

            return await _userManager.CreateAsync(user, dto.Password);
        }

        public async Task<IdentityResult> UpdateUserAsync(int userId, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı bulunamadı" });

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.Phone;
            user.IsActive = dto.IsActive;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı bulunamadı" });

            return await _userManager.DeleteAsync(user);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _userManager.FindByIdAsync(userId.ToString());
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<List<User>> GetUsersByTypeAsync(UserType userType)
        {
            return await _context.Users
                .Where(u => u.UserType == userType && u.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .Where(u => u.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "Kullanıcı bulunamadı" });

            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }
    }
}