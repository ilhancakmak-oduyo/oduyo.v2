using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class TenantUserService : ITenantUserService
    {
        private readonly ApplicationDbContext _context;

        public TenantUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AssignTenantToUserAsync(int tenantId, int userId)
        {
            // Zaten atanmış mı kontrol et
            var exists = await _context.TenantUsers
                .AnyAsync(tu => tu.TenantId == tenantId && tu.UserId == userId);

            if (exists)
                return false;

            var tenantUser = new TenantUser
            {
                TenantId = tenantId,
                UserId = userId
            };

            _context.TenantUsers.Add(tenantUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveTenantFromUserAsync(int tenantId, int userId)
        {
            var tenantUser = await _context.TenantUsers
                .FirstOrDefaultAsync(tu => tu.TenantId == tenantId && tu.UserId == userId);

            if (tenantUser == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Tenant>> GetUserTenantsAsync(int userId)
        {
            return await _context.TenantUsers
                .Where(tu => tu.UserId == userId)
                .Join(_context.Tenants, tu => tu.TenantId, t => t.Id, (tu, t) => t)
                .ToListAsync();
        }

        public async Task<List<User>> GetTenantUsersAsync(int tenantId)
        {
            return await _context.TenantUsers
                .Where(tu => tu.TenantId == tenantId)
                .Join(_context.Users, tu => tu.UserId, u => u.Id, (tu, u) => u)
                .ToListAsync();
        }

        public async Task<bool> IsUserAssignedToTenantAsync(int tenantId, int userId)
        {
            return await _context.TenantUsers
                .AnyAsync(tu => tu.TenantId == tenantId && tu.UserId == userId);
        }
    }
}