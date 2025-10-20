using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ITenantUserService
    {
        Task<bool> AssignTenantToUserAsync(int tenantId, int userId);
        Task<bool> RemoveTenantFromUserAsync(int tenantId, int userId);
        Task<List<Tenant>> GetUserTenantsAsync(int userId);
        Task<List<User>> GetTenantUsersAsync(int tenantId);
        Task<bool> IsUserAssignedToTenantAsync(int tenantId, int userId);
    }
}