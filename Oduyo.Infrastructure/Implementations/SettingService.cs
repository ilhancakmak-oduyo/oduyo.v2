using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class SettingService : ISettingService
    {
        private readonly ApplicationDbContext _context;

        public SettingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Setting> CreateSettingAsync(CreateSettingDto dto)
        {
            var setting = new Setting
            {
                Key = dto.Key,
                Value = dto.Value,
                Description = dto.Description
            };

            _context.Settings.Add(setting);
            await _context.SaveChangesAsync();

            return setting;
        }

        public async Task<Setting> UpdateSettingAsync(int settingId, UpdateSettingDto dto)
        {
            var setting = await _context.Settings.FindAsync(settingId);
            if (setting == null)
                throw new InvalidOperationException("Ayar bulunamadı.");

            setting.Value = dto.Value;
            setting.Description = dto.Description;

            await _context.SaveChangesAsync();
            return setting;
        }

        public async Task<bool> DeleteSettingAsync(int settingId)
        {
            var setting = await _context.Settings.FindAsync(settingId);
            if (setting == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Setting> GetSettingByIdAsync(int settingId)
        {
            return await _context.Settings.FirstOrDefaultAsync(s => s.Id == settingId);
        }

        public async Task<Setting> GetSettingByKeyAsync(string key)
        {
            return await _context.Settings.FirstOrDefaultAsync(s => s.Key == key);
        }

        public async Task<List<Setting>> GetAllSettingsAsync()
        {
            return await _context.Settings.ToListAsync();
        }

        public async Task<string> GetSettingValueAsync(string key, string defaultValue = null)
        {
            var setting = await GetSettingByKeyAsync(key);
            return setting?.Value ?? defaultValue;
        }
    }
}