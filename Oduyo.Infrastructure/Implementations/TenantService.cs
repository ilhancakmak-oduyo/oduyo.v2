using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class TenantService : ITenantService
    {
        private readonly ApplicationDbContext _context;

        public TenantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Tenant> CreateTenantAsync(CreateTenantDto dto)
        {
            var existingTenant = await _context.Tenants
                .Where(t => t.Domain == dto.Domain)
                .FirstOrDefaultAsync();

            if (existingTenant != null)
                throw new InvalidOperationException("Bu domain zaten kullanılıyor.");

            var tenant = new Tenant
            {
                Name = dto.Name,
                Domain = dto.Domain,
                ConnectionString = dto.ConnectionString,
                IsActive = true
            };

            _context.Tenants.Add(tenant);
            await _context.SaveChangesAsync();
            return tenant;
        }

        public async Task<Tenant> UpdateTenantAsync(int tenantId, UpdateTenantDto dto)
        {
            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant == null)
                throw new InvalidOperationException("Tenant bulunamadı.");

            // Domain değişikliği kontrolü
            if (tenant.Domain != dto.Domain)
            {
                var existingTenant = await _context.Tenants
                    .Where(t => t.Domain == dto.Domain && t.Id != tenantId)
                    .FirstOrDefaultAsync();

                if (existingTenant != null)
                    throw new InvalidOperationException("Bu domain zaten kullanılıyor.");
            }

            tenant.Name = dto.Name;
            tenant.Domain = dto.Domain;
            tenant.ConnectionString = dto.ConnectionString;
            tenant.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return tenant;
        }

        public async Task<bool> DeleteTenantAsync(int tenantId)
        {
            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Tenant> GetTenantByIdAsync(int tenantId)
        {
            return await _context.Tenants
                .FirstOrDefaultAsync(t => t.Id == tenantId);
        }

        public async Task<Tenant> GetTenantByDomainAsync(string domain)
        {
            return await _context.Tenants
                .FirstOrDefaultAsync(t => t.Domain == domain);
        }

        public async Task<List<Tenant>> GetAllTenantsAsync()
        {
            return await _context.Tenants.ToListAsync();
        }

        public async Task<List<Tenant>> GetActiveTenantsAsync()
        {
            return await _context.Tenants
                .Where(t => t.IsActive)
                .ToListAsync();
        }
    }
}