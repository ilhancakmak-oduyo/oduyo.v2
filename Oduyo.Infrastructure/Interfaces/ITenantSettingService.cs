using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ITenantSettingService
    {
        Task<TenantSetting> CreateSettingAsync(CreateTenantSettingDto dto);
        Task<TenantSetting> UpdateSettingAsync(int settingId, UpdateTenantSettingDto dto);
        Task<bool> DeleteSettingAsync(int settingId);
        Task<TenantSetting> GetSettingByIdAsync(int settingId);
        Task<List<TenantSetting>> GetTenantSettingsAsync(int tenantId);
        Task<TenantSetting> GetTenantSettingByKeyAsync(int tenantId, string key);
        Task<string> GetTenantSettingValueAsync(int tenantId, string key, string defaultValue = null);
    }
}