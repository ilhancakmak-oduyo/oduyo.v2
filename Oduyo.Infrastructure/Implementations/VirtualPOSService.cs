using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class VirtualPOSService : IVirtualPOSService
    {
        private readonly ApplicationDbContext _context;

        public VirtualPOSService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<VirtualPOS> CreateVirtualPOSAsync(CreateVirtualPOSDto dto)
        {
            var virtualPOS = new VirtualPOS
            {
                Name = dto.Name,
                MerchantId = dto.MerchantId,
                ApiKey = dto.ApiKey,
                ApiSecret = dto.ApiSecret,
                IsActive = true
            };

            _context.VirtualPOSs.Add(virtualPOS);
            await _context.SaveChangesAsync();

            return virtualPOS;
        }

        public async Task<VirtualPOS> UpdateVirtualPOSAsync(int virtualPOSId, UpdateVirtualPOSDto dto)
        {
            var virtualPOS = await _context.VirtualPOSs.FindAsync(virtualPOSId);
            if (virtualPOS == null)
                throw new InvalidOperationException("Sanal POS bulunamadı.");

            virtualPOS.Name = dto.Name;
            virtualPOS.MerchantId = dto.MerchantId;
            virtualPOS.ApiKey = dto.ApiKey;
            virtualPOS.ApiSecret = dto.ApiSecret;
            virtualPOS.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return virtualPOS;
        }

        public async Task<bool> DeleteVirtualPOSAsync(int virtualPOSId)
        {
            var virtualPOS = await _context.VirtualPOSs.FindAsync(virtualPOSId);
            if (virtualPOS == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<VirtualPOS> GetVirtualPOSByIdAsync(int virtualPOSId)
        {
            return await _context.VirtualPOSs.FirstOrDefaultAsync(v => v.Id == virtualPOSId);
        }

        public async Task<List<VirtualPOS>> GetAllVirtualPOSAsync()
        {
            return await _context.VirtualPOSs.ToListAsync();
        }

        public async Task<List<VirtualPOS>> GetActiveVirtualPOSAsync()
        {
            return await _context.VirtualPOSs.Where(v => v.IsActive).ToListAsync();
        }
    }
}