using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IModuleService
    {
        Task<Module> CreateModuleAsync(CreateModuleDto dto);
        Task<Module> UpdateModuleAsync(int moduleId, UpdateModuleDto dto);
        Task<bool> DeleteModuleAsync(int moduleId);
        Task<Module> GetModuleByIdAsync(int moduleId);
        Task<List<Module>> GetAllModulesAsync();
        Task<List<Module>> GetActiveModulesAsync();
    }
}