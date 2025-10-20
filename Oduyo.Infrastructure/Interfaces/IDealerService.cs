using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IDealerService
    {
        Task<Dealer> CreateDealerAsync(CreateDealerDto dto);
        Task<Dealer> UpdateDealerAsync(int dealerId, UpdateDealerDto dto);
        Task<bool> DeleteDealerAsync(int dealerId);
        Task<Dealer> GetDealerByIdAsync(int dealerId);
        Task<List<Dealer>> GetAllDealersAsync();
        Task<List<Dealer>> GetActiveDealersAsync();
    }
}