using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IProductPriceService
    {
        Task<ProductPrice> CreatePriceAsync(int productId, decimal price, int currencyId, DateTime effectiveFrom);
        Task<ProductPrice> GetCurrentPriceAsync(int productId, int currencyId);
        Task<List<ProductPrice>> GetProductPriceHistoryAsync(int productId);
    }
}