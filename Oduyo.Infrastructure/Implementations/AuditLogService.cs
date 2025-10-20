using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class AuditLogService : IAuditLogService
    {
        private readonly ApplicationDbContext _context;

        public AuditLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<AuditLog> CreateAuditLogAsync(CreateAuditLogDto dto)
        {
            var auditLog = new AuditLog
            {
                UserId = dto.UserId,
                Action = dto.Action,
                Entity = dto.Entity,
                EntityId = dto.EntityId,
                OldValues = dto.OldValues,
                NewValues = dto.NewValues,
                IpAddress = dto.IpAddress
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();

            return auditLog;
        }

        public async Task<List<AuditLog>> GetUserAuditLogsAsync(int userId)
        {
            return await _context.AuditLogs
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<AuditLog>> GetEntityAuditLogsAsync(string entity, int entityId)
        {
            return await _context.AuditLogs
                .Where(a => a.Entity == entity && a.EntityId == entityId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<AuditLog>> GetAuditLogsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.AuditLogs
                .Where(a => a.CreatedAt >= startDate && a.CreatedAt <= endDate)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}