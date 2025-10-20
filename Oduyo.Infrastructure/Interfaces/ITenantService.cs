using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ITenantService
    {
        Task<Tenant> CreateTenantAsync(CreateTenantDto dto);
        Task<Tenant> UpdateTenantAsync(int tenantId, UpdateTenantDto dto);
        Task<bool> DeleteTenantAsync(int tenantId);
        Task<Tenant> GetTenantByIdAsync(int tenantId);
        Task<Tenant> GetTenantByDomainAsync(string domain);
        Task<List<Tenant>> GetAllTenantsAsync();
        Task<List<Tenant>> GetActiveTenantsAsync();
    }
}