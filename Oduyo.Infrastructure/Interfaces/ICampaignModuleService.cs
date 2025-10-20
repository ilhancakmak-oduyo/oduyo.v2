using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ICampaignModuleService
    {
        Task<bool> AssignModuleToCampaignAsync(int campaignId, int moduleId);
        Task<bool> RemoveModuleFromCampaignAsync(int campaignId, int moduleId);
        Task<List<Module>> GetCampaignModulesAsync(int campaignId);
        Task<bool> IsModuleInCampaignAsync(int campaignId, int moduleId);
    }
}