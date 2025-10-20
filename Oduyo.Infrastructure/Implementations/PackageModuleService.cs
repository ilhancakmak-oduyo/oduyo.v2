using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class PackageModuleService : IPackageModuleService
    {
        private readonly ApplicationDbContext _context;

        public PackageModuleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AssignModuleToPackageAsync(int packageId, int moduleId, bool isFree)
        {
            var exists = await _context.PackageModules
                .AnyAsync(pm => pm.PackageId == packageId && pm.ModuleId == moduleId);

            if (exists)
                return false;

            var packageModule = new PackageModule
            {
                PackageId = packageId,
                ModuleId = moduleId,
                IsFree = isFree
            };

            _context.PackageModules.Add(packageModule);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveModuleFromPackageAsync(int packageId, int moduleId)
        {
            var packageModule = await _context.PackageModules
                .FirstOrDefaultAsync(pm => pm.PackageId == packageId && pm.ModuleId == moduleId);

            if (packageModule == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Module>> GetPackageModulesAsync(int packageId)
        {
            return await _context.PackageModules
                .Where(pm => pm.PackageId == packageId)
                .Join(_context.Modules, pm => pm.ModuleId, m => m.Id, (pm, m) => m)
                .ToListAsync();
        }

        public async Task<List<Module>> GetPackageFreeModulesAsync(int packageId)
        {
            return await _context.PackageModules
                .Where(pm => pm.PackageId == packageId && pm.IsFree)
                .Join(_context.Modules, pm => pm.ModuleId, m => m.Id, (pm, m) => m)
                .ToListAsync();
        }

        public async Task<bool> IsModuleInPackageAsync(int packageId, int moduleId)
        {
            return await _context.PackageModules
                .AnyAsync(pm => pm.PackageId == packageId && pm.ModuleId == moduleId);
        }
    }
}