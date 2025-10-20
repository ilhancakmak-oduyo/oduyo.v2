using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class TenantSettingService : ITenantSettingService
    {
        private readonly ApplicationDbContext _context;

        public TenantSettingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TenantSetting> CreateSettingAsync(CreateTenantSettingDto dto)
        {
            // Aynı key zaten var mı kontrol et
            var exists = await _context.TenantSettings
                .AnyAsync(ts => ts.TenantId == dto.TenantId && ts.Key == dto.Key);

            if (exists)
                throw new InvalidOperationException("Bu ayar zaten mevcut. Güncelleme için UpdateSettingAsync kullanın.");

            var setting = new TenantSetting
            {
                TenantId = dto.TenantId,
                Key = dto.Key,
                Value = dto.Value
            };

            _context.TenantSettings.Add(setting);
            await _context.SaveChangesAsync();
            return setting;
        }

        public async Task<TenantSetting> UpdateSettingAsync(int settingId, UpdateTenantSettingDto dto)
        {
            var setting = await _context.TenantSettings.FindAsync(settingId);
            if (setting == null)
                throw new InvalidOperationException("Ayar bulunamadı.");

            setting.Value = dto.Value;

            await _context.SaveChangesAsync();
            return setting;
        }

        public async Task<bool> DeleteSettingAsync(int settingId)
        {
            var setting = await _context.TenantSettings.FindAsync(settingId);
            if (setting == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TenantSetting> GetSettingByIdAsync(int settingId)
        {
            return await _context.TenantSettings
                .FirstOrDefaultAsync(ts => ts.Id == settingId);
        }

        public async Task<List<TenantSetting>> GetTenantSettingsAsync(int tenantId)
        {
            return await _context.TenantSettings
                .Where(ts => ts.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task<TenantSetting> GetTenantSettingByKeyAsync(int tenantId, string key)
        {
            return await _context.TenantSettings
                .FirstOrDefaultAsync(ts => ts.TenantId == tenantId && ts.Key == key);
        }

        public async Task<string> GetTenantSettingValueAsync(int tenantId, string key, string defaultValue = null)
        {
            var setting = await GetTenantSettingByKeyAsync(tenantId, key);
            return setting?.Value ?? defaultValue;
        }
    }
}