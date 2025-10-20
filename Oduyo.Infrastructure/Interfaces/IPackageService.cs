using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IPackageService
    {
        Task<Package> CreatePackageAsync(CreatePackageDto dto);
        Task<Package> UpdatePackageAsync(int packageId, UpdatePackageDto dto);
        Task<bool> DeletePackageAsync(int packageId);
        Task<Package> GetPackageByIdAsync(int packageId);
        Task<List<Package>> GetAllPackagesAsync();
        Task<List<Package>> GetActivePackagesAsync();
        Task<List<Package>> GetPackagesByProductAsync(int productId);
    }
}