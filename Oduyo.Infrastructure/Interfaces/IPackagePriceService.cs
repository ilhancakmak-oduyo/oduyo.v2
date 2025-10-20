using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IPackagePriceService
    {
        Task<PackagePrice> CreatePriceAsync(int packageId, decimal price, int currencyId, DateTime effectiveFrom);
        Task<PackagePrice> GetCurrentPriceAsync(int packageId, int currencyId);
        Task<List<PackagePrice>> GetPackagePriceHistoryAsync(int packageId);
    }
}