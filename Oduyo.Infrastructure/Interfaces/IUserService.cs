using Microsoft.AspNetCore.Identity;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(CreateUserDto dto);
        Task<IdentityResult> UpdateUserAsync(int userId, UpdateUserDto dto);
        Task<IdentityResult> DeleteUserAsync(int userId);
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<List<User>> GetUsersByTypeAsync(UserType userType);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}