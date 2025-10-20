using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class ModuleService : IModuleService
    {
        private readonly ApplicationDbContext _context;

        public ModuleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Module> CreateModuleAsync(CreateModuleDto dto)
        {
            var module = new Module
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsActive = true
            };

            _context.Modules.Add(module);
            await _context.SaveChangesAsync();
            return module;
        }

        public async Task<Module> UpdateModuleAsync(int moduleId, UpdateModuleDto dto)
        {
            var module = await _context.Modules.FindAsync(moduleId);
            if (module == null)
                throw new InvalidOperationException("Modül bulunamadı.");

            module.Name = dto.Name;
            module.Description = dto.Description;
            module.Price = dto.Price;
            module.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return module;
        }

        public async Task<bool> DeleteModuleAsync(int moduleId)
        {
            var module = await _context.Modules.FindAsync(moduleId);
            if (module == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Module> GetModuleByIdAsync(int moduleId)
        {
            return await _context.Modules.FirstOrDefaultAsync(m => m.Id == moduleId);
        }

        public async Task<List<Module>> GetAllModulesAsync()
        {
            return await _context.Modules.ToListAsync();
        }

        public async Task<List<Module>> GetActiveModulesAsync()
        {
            return await _context.Modules.Where(m => m.IsActive).ToListAsync();
        }
    }
}