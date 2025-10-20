using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class UserHierarchyService : IUserHierarchyService
    {
        private readonly ApplicationDbContext _context;

        public UserHierarchyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserHierarchy> CreateHierarchyAsync(int parentUserId, int childUserId)
        {
            var exists = await _context.UserHierarchies
                .AnyAsync(uh => uh.ParentUserId == parentUserId && uh.ChildUserId == childUserId && uh.DeletedAt == null);

            if (exists) return null;

            var hierarchy = new UserHierarchy
            {
                ParentUserId = parentUserId,
                ChildUserId = childUserId
            };

            _context.UserHierarchies.Add(hierarchy);
            await _context.SaveChangesAsync();
            return hierarchy;
        }

        public async Task<bool> DeleteHierarchyAsync(int hierarchyId)
        {
            var hierarchy = await _context.UserHierarchies.FindAsync(hierarchyId);
            if (hierarchy == null) return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetChildUsersAsync(int parentUserId)
        {
            var childIds = await _context.UserHierarchies
                .Where(uh => uh.ParentUserId == parentUserId && uh.DeletedAt == null)
                .Select(uh => uh.ChildUserId)
                .ToListAsync();

            return await _context.Users
                .Where(u => childIds.Contains(u.Id) && u.DeletedAt == null)
                .ToListAsync();
        }

        public async Task<User> GetParentUserAsync(int childUserId)
        {
            var parentId = await _context.UserHierarchies
                .Where(uh => uh.ChildUserId == childUserId && uh.DeletedAt == null)
                .Select(uh => uh.ParentUserId)
                .FirstOrDefaultAsync();

            if (parentId == 0) return null;

            return await _context.Users
                .Where(u => u.Id == parentId && u.DeletedAt == null)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserChildOfAsync(int parentUserId, int childUserId)
        {
            return await _context.UserHierarchies
                .AnyAsync(uh => uh.ParentUserId == parentUserId && uh.ChildUserId == childUserId && uh.DeletedAt == null);
        }
    }
}