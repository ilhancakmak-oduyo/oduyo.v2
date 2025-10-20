using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ICampaignService
    {
        Task<Campaign> CreateCampaignAsync(CreateCampaignDto dto);
        Task<Campaign> UpdateCampaignAsync(int campaignId, UpdateCampaignDto dto);
        Task<bool> DeleteCampaignAsync(int campaignId);
        Task<Campaign> GetCampaignByIdAsync(int campaignId);
        Task<List<Campaign>> GetAllCampaignsAsync();
        Task<List<Campaign>> GetActiveCampaignsAsync();
        Task<List<Campaign>> GetCurrentCampaignsAsync(); // Şu anda geçerli kampanyalar
    }
}