using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IUserHierarchyService
    {
        Task<UserHierarchy> CreateHierarchyAsync(int parentUserId, int childUserId);
        Task<bool> DeleteHierarchyAsync(int hierarchyId);
        Task<List<User>> GetChildUsersAsync(int parentUserId);
        Task<User> GetParentUserAsync(int childUserId);
        Task<bool> IsUserChildOfAsync(int parentUserId, int childUserId);
    }
}