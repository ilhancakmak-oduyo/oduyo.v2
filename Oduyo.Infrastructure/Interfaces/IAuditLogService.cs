using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IAuditLogService
    {
        Task<AuditLog> CreateAuditLogAsync(CreateAuditLogDto dto);
        Task<List<AuditLog>> GetUserAuditLogsAsync(int userId);
        Task<List<AuditLog>> GetEntityAuditLogsAsync(string entity, int entityId);
        Task<List<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}