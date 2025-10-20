using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class DiscountAuthorityService : IDiscountAuthorityService
    {
        private readonly ApplicationDbContext _context;

        public DiscountAuthorityService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DiscountAuthority> CreateAuthorityAsync(CreateDiscountAuthorityDto dto)
        {
            var authority = new DiscountAuthority
            {
                UserId = dto.UserId,
                MaxDiscountRate = dto.MaxDiscountRate,
                IsActive = true
            };

            _context.DiscountAuthorities.Add(authority);
            await _context.SaveChangesAsync();
            return authority;
        }

        public async Task<DiscountAuthority> UpdateAuthorityAsync(int authorityId, UpdateDiscountAuthorityDto dto)
        {
            var authority = await _context.DiscountAuthorities.FindAsync(authorityId);
            if (authority == null) return null;

            authority.MaxDiscountRate = dto.MaxDiscountRate;
            authority.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return authority;
        }

        public async Task<bool> DeleteAuthorityAsync(int authorityId)
        {
            var authority = await _context.DiscountAuthorities.FindAsync(authorityId);
            if (authority == null) return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<DiscountAuthority> GetUserDiscountAuthorityAsync(int userId)
        {
            return await _context.DiscountAuthorities
                .Where(da => da.UserId == userId && da.IsActive && da.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> CanUserApplyDiscountAsync(int userId, decimal discountRate)
        {
            var authority = await GetUserDiscountAuthorityAsync(userId);
            return authority != null && authority.MaxDiscountRate >= discountRate;
        }
    }
}