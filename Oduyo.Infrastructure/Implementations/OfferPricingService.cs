using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Constants;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class OfferPricingService : IOfferPricingService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDiscountAuthorityService _discountAuthority;
        private readonly ILogger<OfferPricingService> _logger;

        public OfferPricingService(
            ApplicationDbContext context,
            IDiscountAuthorityService discountAuthority,
            ILogger<OfferPricingService> logger)
        {
            _context = context;
            _discountAuthority = discountAuthority;
            _logger = logger;
        }

        public async Task<OfferPriceCalculation> CalculatePricingAsync(OfferPricingRequest request)
        {
            var calculation = new OfferPriceCalculation();

            // 1. Calculate subtotal
            foreach (var detail in request.Details)
            {
                var packagePrice = await GetPackagePriceAsync(detail.PackageId);
                var modulePrice = await GetModulePriceAsync(detail.ModuleId);

                var price = detail.PackageId.HasValue ? packagePrice : modulePrice;
                calculation.SubTotal += price * detail.Quantity;
            }

            // 2. Apply campaign discount if eligible
            if (request.CampaignId.HasValue)
            {
                var campaign = await _context.Campaigns
                    .FirstOrDefaultAsync(c => c.Id == request.CampaignId.Value);

                if (campaign != null)
                {
                    calculation.IsCampaignActive = campaign.IsCurrentlyActive;
                    calculation.IsCampaignFrozen = campaign.IsWithinFreezePeriod;

                    // Apply discount if active or frozen
                    if (calculation.IsCampaignActive || calculation.IsCampaignFrozen)
                    {
                        calculation.CampaignDiscount = await CalculateCampaignDiscountAsync(
                            campaign,
                            request.Details
                        );
                    }
                }
            }

            // 3. Apply cross-sell perks
            calculation.CrossSellDiscount = await CalculateCrossSellPerksAsync(request.Details);

            // 4. Apply manual discount (with authorization)
            if (request.ManualDiscountRate > 0)
            {
                var canApply = await _discountAuthority.CanUserApplyDiscountAsync(
                    request.UserId,
                    request.ManualDiscountRate
                );

                if (!canApply)
                {
                    throw new UnauthorizedAccessException(
                        $"Kullanıcı %{request.ManualDiscountRate} indirim yetkisine sahip değil."
                    );
                }

                var discountableAmount = calculation.SubTotal - calculation.CampaignDiscount;
                calculation.ManualDiscount = discountableAmount * (request.ManualDiscountRate / 100);
            }

            // 5. Calculate final price
            calculation.FinalPrice = calculation.SubTotal
                - calculation.CampaignDiscount
                - calculation.CrossSellDiscount
                - calculation.ManualDiscount;

            _logger.LogInformation(
                "Pricing calculated: SubTotal={SubTotal}, Campaign={Campaign}, Manual={Manual}, Final={Final}",
                calculation.SubTotal, calculation.CampaignDiscount, calculation.ManualDiscount, calculation.FinalPrice
            );

            return calculation;
        }

        private async Task<decimal> CalculateCampaignDiscountAsync(
            Domain.Entities.Campaign campaign,
            IEnumerable<OfferDetailDto> details)
        {
            decimal totalDiscount = 0;

            foreach (var detail in details)
            {
                // Check if package is in campaign
                bool isEligible = false;

                if (detail.PackageId.HasValue)
                {
                    isEligible = await _context.CampaignPackages
                        .AnyAsync(cp => cp.CampaignId == campaign.Id && cp.PackageId == detail.PackageId.Value);
                }
                else if (detail.ModuleId.HasValue)
                {
                    isEligible = await _context.CampaignModules
                        .AnyAsync(cm => cm.CampaignId == campaign.Id && cm.ModuleId == detail.ModuleId.Value);
                }

                if (isEligible)
                {
                    var price = detail.PackageId.HasValue
                        ? await GetPackagePriceAsync(detail.PackageId)
                        : await GetModulePriceAsync(detail.ModuleId);

                    if (campaign.DiscountRate.HasValue)
                    {
                        totalDiscount += price * detail.Quantity * (campaign.DiscountRate.Value / 100);
                    }
                    else if (campaign.DiscountAmount.HasValue)
                    {
                        totalDiscount += campaign.DiscountAmount.Value * detail.Quantity;
                    }
                }
            }

            return totalDiscount;
        }

        public async Task<decimal> CalculateCrossSellPerksAsync(IEnumerable<OfferDetailDto> details)
        {
            decimal totalPerkValue = 0;

            // Example: Enterprise Tahsilat includes free Ekstre Lite module
            var hasEnterprisePackage = details.Any(d =>
                d.PackageId == KnownPackages.OnlineTahsilatEnterprise);

            if (hasEnterprisePackage)
            {
                var ekstreLitePrice = await GetModulePriceAsync(KnownModules.EkstreLite);
                totalPerkValue += ekstreLitePrice;

                _logger.LogInformation("Cross-sell perk applied: Free Ekstre Lite with Enterprise package");
            }

            // Add more cross-sell rules here as needed

            return totalPerkValue;
        }

        public async Task<bool> ValidateCampaignEligibilityAsync(int campaignId, IEnumerable<int> packageIds)
        {
            var campaign = await _context.Campaigns
                .Include(c => c.CampaignPackages)
                .FirstOrDefaultAsync(c => c.Id == campaignId);

            if (campaign == null || !campaign.IsCurrentlyActive)
                return false;

            // Check if at least one package is eligible
            var campaignPackageIds = campaign.CampaignPackages.Select(cp => cp.PackageId).ToList();
            return packageIds.Any(pid => campaignPackageIds.Contains(pid));
        }

        private async Task<decimal> GetPackagePriceAsync(int? packageId)
        {
            if (!packageId.HasValue)
                return 0;

            var price = await _context.PackagePrices
                .Where(pp => pp.PackageId == packageId.Value && pp.IsActive)
                .OrderByDescending(pp => pp.CreatedAt)
                .FirstOrDefaultAsync();

            return price?.Price ?? 0;
        }

        private async Task<decimal> GetModulePriceAsync(int? moduleId)
        {
            if (!moduleId.HasValue)
                return 0;

            // Assuming modules have a base price field
            var module = await _context.Modules.FindAsync(moduleId.Value);
            return module?.BasePrice ?? 0;
        }
    }
}
