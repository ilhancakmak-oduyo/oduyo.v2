using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ICampaignPackageService
    {
        Task<bool> AssignPackageToCampaignAsync(int campaignId, int packageId);
        Task<bool> RemovePackageFromCampaignAsync(int campaignId, int packageId);
        Task<List<Package>> GetCampaignPackagesAsync(int campaignId);
        Task<bool> IsPackageInCampaignAsync(int campaignId, int packageId);
    }
}