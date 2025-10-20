using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class CampaignService : ICampaignService
    {
        private readonly ApplicationDbContext _context;

        public CampaignService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Campaign> CreateCampaignAsync(CreateCampaignDto dto)
        {
            var campaign = new Campaign
            {
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                DiscountAmount = dto.DiscountAmount,
                DiscountRate = dto.DiscountRate,
                Description = dto.Description,
                IsActive = true
            };

            _context.Campaigns.Add(campaign);
            await _context.SaveChangesAsync();
            return campaign;
        }

        public async Task<Campaign> UpdateCampaignAsync(int campaignId, UpdateCampaignDto dto)
        {
            var campaign = await _context.Campaigns.FindAsync(campaignId);
            if (campaign == null)
                throw new InvalidOperationException("Kampanya bulunamadı.");

            campaign.Name = dto.Name;
            campaign.StartDate = dto.StartDate;
            campaign.EndDate = dto.EndDate;
            campaign.DiscountAmount = dto.DiscountAmount;
            campaign.DiscountRate = dto.DiscountRate;
            campaign.Description = dto.Description;
            campaign.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return campaign;
        }

        public async Task<bool> DeleteCampaignAsync(int campaignId)
        {
            var campaign = await _context.Campaigns.FindAsync(campaignId);
            if (campaign == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Campaign> GetCampaignByIdAsync(int campaignId)
        {
            return await _context.Campaigns.FirstOrDefaultAsync(c => c.Id == campaignId);
        }

        public async Task<List<Campaign>> GetAllCampaignsAsync()
        {
            return await _context.Campaigns.ToListAsync();
        }

        public async Task<List<Campaign>> GetActiveCampaignsAsync()
        {
            return await _context.Campaigns.Where(c => c.IsActive).ToListAsync();
        }

        public async Task<List<Campaign>> GetCurrentCampaignsAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Campaigns
                .Where(c => c.IsActive && c.StartDate <= now && c.EndDate >= now)
                .ToListAsync();
        }
    }
}