using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IVirtualPOSService
    {
        Task<VirtualPOS> CreateVirtualPOSAsync(CreateVirtualPOSDto dto);
        Task<VirtualPOS> UpdateVirtualPOSAsync(int virtualPOSId, UpdateVirtualPOSDto dto);
        Task<bool> DeleteVirtualPOSAsync(int virtualPOSId);
        Task<VirtualPOS> GetVirtualPOSByIdAsync(int virtualPOSId);
        Task<List<VirtualPOS>> GetAllVirtualPOSAsync();
        Task<List<VirtualPOS>> GetActiveVirtualPOSAsync();
    }
}