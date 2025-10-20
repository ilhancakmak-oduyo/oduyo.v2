using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class StatusService : IStatusService
    {
        private readonly ApplicationDbContext _context;

        public StatusService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Status> CreateStatusAsync(CreateStatusDto dto)
        {
            var status = new Status
            {
                Name = dto.Name,
                EntityType = dto.EntityType,
                IsActive = true
            };

            _context.Statuses.Add(status);
            await _context.SaveChangesAsync();
            return status;
        }

        public async Task<Status> UpdateStatusAsync(int statusId, UpdateStatusDto dto)
        {
            var status = await _context.Statuses.FindAsync(statusId);
            if (status == null)
                throw new InvalidOperationException("Durum bulunamadı.");

            status.Name = dto.Name;
            status.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return status;
        }

        public async Task<bool> DeleteStatusAsync(int statusId)
        {
            var status = await _context.Statuses.FindAsync(statusId);
            if (status == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Status> GetStatusByIdAsync(int statusId)
        {
            return await _context.Statuses.FirstOrDefaultAsync(s => s.Id == statusId);
        }

        public async Task<List<Status>> GetAllStatusesAsync()
        {
            return await _context.Statuses.ToListAsync();
        }

        public async Task<List<Status>> GetActiveStatusesAsync()
        {
            return await _context.Statuses.Where(s => s.IsActive).ToListAsync();
        }

        public async Task<List<Status>> GetStatusesByEntityTypeAsync(string entityType)
        {
            return await _context.Statuses
                .Where(s => s.EntityType == entityType && s.IsActive)
                .ToListAsync();
        }
    }
}