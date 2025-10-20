using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class CampaignModuleService : ICampaignModuleService
    {
        private readonly ApplicationDbContext _context;

        public CampaignModuleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AssignModuleToCampaignAsync(int campaignId, int moduleId)
        {
            var exists = await _context.CampaignModules
                .AnyAsync(cm => cm.CampaignId == campaignId && cm.ModuleId == moduleId);

            if (exists)
                return false;

            var campaignModule = new CampaignModule
            {
                CampaignId = campaignId,
                ModuleId = moduleId
            };

            _context.CampaignModules.Add(campaignModule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveModuleFromCampaignAsync(int campaignId, int moduleId)
        {
            var campaignModule = await _context.CampaignModules
                .FirstOrDefaultAsync(cm => cm.CampaignId == campaignId && cm.ModuleId == moduleId);

            if (campaignModule == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Module>> GetCampaignModulesAsync(int campaignId)
        {
            return await _context.CampaignModules
                .Where(cm => cm.CampaignId == campaignId)
                .Join(_context.Modules, cm => cm.ModuleId, m => m.Id, (cm, m) => m)
                .ToListAsync();
        }

        public async Task<bool> IsModuleInCampaignAsync(int campaignId, int moduleId)
        {
            return await _context.CampaignModules
                .AnyAsync(cm => cm.CampaignId == campaignId && cm.ModuleId == moduleId);
        }
    }
}