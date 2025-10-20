using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class PackageService : IPackageService
    {
        private readonly ApplicationDbContext _context;

        public PackageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Package> CreatePackageAsync(CreatePackageDto dto)
        {
            var package = new Package
            {
                ProductId = dto.ProductId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsActive = true
            };

            _context.Packages.Add(package);
            await _context.SaveChangesAsync();
            return package;
        }

        public async Task<Package> UpdatePackageAsync(int packageId, UpdatePackageDto dto)
        {
            var package = await _context.Packages.FindAsync(packageId);
            if (package == null)
                throw new InvalidOperationException("Paket bulunamadı.");

            package.Name = dto.Name;
            package.Description = dto.Description;
            package.Price = dto.Price;
            package.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return package;
        }

        public async Task<bool> DeletePackageAsync(int packageId)
        {
            var package = await _context.Packages.FindAsync(packageId);
            if (package == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Package> GetPackageByIdAsync(int packageId)
        {
            return await _context.Packages.FirstOrDefaultAsync(p => p.Id == packageId);
        }

        public async Task<List<Package>> GetAllPackagesAsync()
        {
            return await _context.Packages.ToListAsync();
        }

        public async Task<List<Package>> GetActivePackagesAsync()
        {
            return await _context.Packages.Where(p => p.IsActive).ToListAsync();
        }

        public async Task<List<Package>> GetPackagesByProductAsync(int productId)
        {
            return await _context.Packages
                .Where(p => p.ProductId == productId)
                .ToListAsync();
        }
    }
}