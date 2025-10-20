using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ISettingService
    {
        Task<Setting> CreateSettingAsync(CreateSettingDto dto);
        Task<Setting> UpdateSettingAsync(int settingId, UpdateSettingDto dto);
        Task<bool> DeleteSettingAsync(int settingId);
        Task<Setting> GetSettingByIdAsync(int settingId);
        Task<Setting> GetSettingByKeyAsync(string key);
        Task<List<Setting>> GetAllSettingsAsync();
        Task<string> GetSettingValueAsync(string key, string defaultValue = null);
    }
}