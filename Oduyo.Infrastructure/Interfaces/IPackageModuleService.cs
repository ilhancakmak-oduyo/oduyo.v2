using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IPackageModuleService
    {
        Task<bool> AssignModuleToPackageAsync(int packageId, int moduleId, bool isFree);
        Task<bool> RemoveModuleFromPackageAsync(int packageId, int moduleId);
        Task<List<Module>> GetPackageModulesAsync(int packageId);
        Task<List<Module>> GetPackageFreeModulesAsync(int packageId);
        Task<bool> IsModuleInPackageAsync(int packageId, int moduleId);
    }
}