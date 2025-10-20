using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class CampaignPackageService : ICampaignPackageService
    {
        private readonly ApplicationDbContext _context;

        public CampaignPackageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AssignPackageToCampaignAsync(int campaignId, int packageId)
        {
            var exists = await _context.CampaignPackages
                .AnyAsync(cp => cp.CampaignId == campaignId && cp.PackageId == packageId);

            if (exists)
                return false;

            var campaignPackage = new CampaignPackage
            {
                CampaignId = campaignId,
                PackageId = packageId
            };

            _context.CampaignPackages.Add(campaignPackage);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemovePackageFromCampaignAsync(int campaignId, int packageId)
        {
            var campaignPackage = await _context.CampaignPackages
                .FirstOrDefaultAsync(cp => cp.CampaignId == campaignId && cp.PackageId == packageId);

            if (campaignPackage == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Package>> GetCampaignPackagesAsync(int campaignId)
        {
            return await _context.CampaignPackages
                .Where(cp => cp.CampaignId == campaignId)
                .Join(_context.Packages, cp => cp.PackageId, p => p.Id, (cp, p) => p)
                .ToListAsync();
        }

        public async Task<bool> IsPackageInCampaignAsync(int campaignId, int packageId)
        {
            return await _context.CampaignPackages
                .AnyAsync(cp => cp.CampaignId == campaignId && cp.PackageId == packageId);
        }
    }
}