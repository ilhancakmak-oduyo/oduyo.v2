using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IDemoService
    {
        Task<Demo> CreateDemoAsync(CreateDemoDto dto);
        Task<Demo> UpdateDemoAsync(int demoId, UpdateDemoDto dto);
        Task<bool> DeleteDemoAsync(int demoId);
        Task<Demo> GetDemoByIdAsync(int demoId);
        Task<List<Demo>> GetAllDemosAsync();
        Task<List<Demo>> GetDemosByCompanyAsync(int companyId);
        Task<List<Demo>> GetDemosByStatusAsync(DemoStatus status);
        Task<List<Demo>> GetDemosByAssignedUserAsync(int assignedUserId);
        Task<List<Demo>> GetUpcomingDemosAsync();
        Task<bool> CompleteDemoAsync(int demoId);
        Task<bool> CancelDemoAsync(int demoId);
        Task<bool> PostponeDemoAsync(int demoId, DateTime newDate);
    }
}