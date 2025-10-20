using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IStatusService
    {
        Task<Status> CreateStatusAsync(CreateStatusDto dto);
        Task<Status> UpdateStatusAsync(int statusId, UpdateStatusDto dto);
        Task<bool> DeleteStatusAsync(int statusId);
        Task<Status> GetStatusByIdAsync(int statusId);
        Task<List<Status>> GetAllStatusesAsync();
        Task<List<Status>> GetActiveStatusesAsync();
        Task<List<Status>> GetStatusesByEntityTypeAsync(string entityType);
    }
}