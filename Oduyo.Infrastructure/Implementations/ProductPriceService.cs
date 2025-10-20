using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class ProductPriceService : IProductPriceService
    {
        private readonly ApplicationDbContext _context;

        public ProductPriceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductPrice> CreatePriceAsync(int productId, decimal price, int currencyId, DateTime effectiveFrom)
        {
            // Önceki fiyatın bitiş tarihini güncelle
            var currentPrice = await GetCurrentPriceAsync(productId, currencyId);
            if (currentPrice != null && currentPrice.EffectiveTo == null)
            {
                currentPrice.EffectiveTo = effectiveFrom.AddSeconds(-1);
            }

            var productPrice = new ProductPrice
            {
                ProductId = productId,
                Price = price,
                CurrencyId = currencyId,
                EffectiveFrom = effectiveFrom,
                EffectiveTo = null
            };

            _context.ProductPrices.Add(productPrice);
            await _context.SaveChangesAsync();
            return productPrice;
        }

        public async Task<ProductPrice> GetCurrentPriceAsync(int productId, int currencyId)
        {
            var now = DateTime.UtcNow;

            return await _context.ProductPrices
                .Where(pp => pp.ProductId == productId &&
                            pp.CurrencyId == currencyId &&
                            pp.EffectiveFrom <= now &&
                            (pp.EffectiveTo == null || pp.EffectiveTo >= now))
                .OrderByDescending(pp => pp.EffectiveFrom)
                .FirstOrDefaultAsync();
        }

        public async Task<List<ProductPrice>> GetProductPriceHistoryAsync(int productId)
        {
            return await _context.ProductPrices
                .Where(pp => pp.ProductId == productId)
                .OrderByDescending(pp => pp.EffectiveFrom)
                .ToListAsync();
        }
    }
}