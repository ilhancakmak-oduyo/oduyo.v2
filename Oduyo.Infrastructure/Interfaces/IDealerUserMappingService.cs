using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IDealerUserMappingService
    {
        Task<bool> AssignDealerToUserAsync(int dealerId, int userId);
        Task<bool> RemoveDealerFromUserAsync(int dealerId, int userId);
        Task<List<Dealer>> GetUserDealersAsync(int userId);
        Task<List<User>> GetDealerUsersAsync(int dealerId);
        Task<bool> IsUserAssignedToDealerAsync(int dealerId, int userId);
    }
}