using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class DealerUserMappingService : IDealerUserMappingService
    {
        private readonly ApplicationDbContext _context;

        public DealerUserMappingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AssignDealerToUserAsync(int dealerId, int userId)
        {
            // Kullanıcı Dealer tipinde mi kontrol et
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.UserType != UserType.Dealer)
                throw new InvalidOperationException("Kullanıcı bulunamadı veya Dealer tipinde değil.");

            var exists = await _context.DealerUserMappings
                .AnyAsync(dum => dum.DealerId == dealerId && dum.UserId == userId);

            if (exists)
                return false;

            var mapping = new DealerUserMapping
            {
                DealerId = dealerId,
                UserId = userId
            };

            _context.DealerUserMappings.Add(mapping);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveDealerFromUserAsync(int dealerId, int userId)
        {
            var mapping = await _context.DealerUserMappings
                .FirstOrDefaultAsync(dum => dum.DealerId == dealerId && dum.UserId == userId);

            if (mapping == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Dealer>> GetUserDealersAsync(int userId)
        {
            return await _context.DealerUserMappings
                .Where(dum => dum.UserId == userId)
                .Join(_context.Dealers, dum => dum.DealerId, d => d.Id, (dum, d) => d)
                .ToListAsync();
        }

        public async Task<List<User>> GetDealerUsersAsync(int dealerId)
        {
            return await _context.DealerUserMappings
                .Where(dum => dum.DealerId == dealerId)
                .Join(_context.Users, dum => dum.UserId, u => u.Id, (dum, u) => u)
                .Where(u => u.UserType == UserType.Dealer)
                .ToListAsync();
        }

        public async Task<bool> IsUserAssignedToDealerAsync(int dealerId, int userId)
        {
            return await _context.DealerUserMappings
                .AnyAsync(dum => dum.DealerId == dealerId && dum.UserId == userId);
        }
    }
}