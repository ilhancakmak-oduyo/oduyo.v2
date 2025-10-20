using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class PackagePriceService : IPackagePriceService
    {
        private readonly ApplicationDbContext _context;

        public PackagePriceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PackagePrice> CreatePriceAsync(int packageId, decimal price, int currencyId, DateTime effectiveFrom)
        {
            // Önceki fiyatın bitiş tarihini güncelle
            var currentPrice = await GetCurrentPriceAsync(packageId, currencyId);
            if (currentPrice != null && currentPrice.EffectiveTo == null)
            {
                currentPrice.EffectiveTo = effectiveFrom.AddSeconds(-1);
            }

            var packagePrice = new PackagePrice
            {
                PackageId = packageId,
                Price = price,
                CurrencyId = currencyId,
                EffectiveFrom = effectiveFrom,
                EffectiveTo = null
            };

            _context.PackagePrices.Add(packagePrice);
            await _context.SaveChangesAsync();
            return packagePrice;
        }

        public async Task<PackagePrice> GetCurrentPriceAsync(int packageId, int currencyId)
        {
            var now = DateTime.UtcNow;

            return await _context.PackagePrices
                .Where(pp => pp.PackageId == packageId &&
                            pp.CurrencyId == currencyId &&
                            pp.EffectiveFrom <= now &&
                            (pp.EffectiveTo == null || pp.EffectiveTo >= now))
                .OrderByDescending(pp => pp.EffectiveFrom)
                .FirstOrDefaultAsync();
        }

        public async Task<List<PackagePrice>> GetPackagePriceHistoryAsync(int packageId)
        {
            return await _context.PackagePrices
                .Where(pp => pp.PackageId == packageId)
                .OrderByDescending(pp => pp.EffectiveFrom)
                .ToListAsync();
        }
    }
}