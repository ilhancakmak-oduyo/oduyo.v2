using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class DemoService : IDemoService
    {
        private readonly ApplicationDbContext _context;

        public DemoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Demo> CreateDemoAsync(CreateDemoDto dto)
        {
            var demo = new Demo
            {
                CompanyId = dto.CompanyId,
                DemoDate = dto.DemoDate,
                Location = dto.Location,
                ProductInterests = dto.ProductInterests,
                Purpose = dto.Purpose,
                Status = DemoStatus.Planned,
                AssignedUserId = dto.AssignedUserId
            };

            _context.Demos.Add(demo);
            await _context.SaveChangesAsync();
            return demo;
        }

        public async Task<Demo> UpdateDemoAsync(int demoId, UpdateDemoDto dto)
        {
            var demo = await _context.Demos.FindAsync(demoId);
            if (demo == null)
                throw new InvalidOperationException("Demo bulunamadı.");

            demo.DemoDate = dto.DemoDate;
            demo.Location = dto.Location;
            demo.ProductInterests = dto.ProductInterests;
            demo.Purpose = dto.Purpose;
            demo.Status = dto.Status;
            demo.AssignedUserId = dto.AssignedUserId;

            await _context.SaveChangesAsync();
            return demo;
        }

        public async Task<bool> DeleteDemoAsync(int demoId)
        {
            var demo = await _context.Demos.FindAsync(demoId);
            if (demo == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Demo> GetDemoByIdAsync(int demoId)
        {
            return await _context.Demos
                .FirstOrDefaultAsync(d => d.Id == demoId);
        }

        public async Task<List<Demo>> GetAllDemosAsync()
        {
            return await _context.Demos
                .OrderByDescending(d => d.DemoDate)
                .ToListAsync();
        }

        public async Task<List<Demo>> GetDemosByCompanyAsync(int companyId)
        {
            return await _context.Demos
                .Where(d => d.CompanyId == companyId)
                .OrderByDescending(d => d.DemoDate)
                .ToListAsync();
        }

        public async Task<List<Demo>> GetDemosByStatusAsync(DemoStatus status)
        {
            return await _context.Demos
                .Where(d => d.Status == status)
                .OrderByDescending(d => d.DemoDate)
                .ToListAsync();
        }

        public async Task<List<Demo>> GetDemosByAssignedUserAsync(int assignedUserId)
        {
            return await _context.Demos
                .Where(d => d.AssignedUserId == assignedUserId)
                .OrderByDescending(d => d.DemoDate)
                .ToListAsync();
        }

        public async Task<List<Demo>> GetUpcomingDemosAsync()
        {
            var today = DateTime.UtcNow.Date;

            return await _context.Demos
                .Where(d => d.Status == DemoStatus.Planned && d.DemoDate >= today)
                .OrderBy(d => d.DemoDate)
                .ToListAsync();
        }

        public async Task<bool> CompleteDemoAsync(int demoId)
        {
            var demo = await _context.Demos.FindAsync(demoId);
            if (demo == null)
                return false;

            demo.Status = DemoStatus.Completed;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelDemoAsync(int demoId)
        {
            var demo = await _context.Demos.FindAsync(demoId);
            if (demo == null)
                return false;

            demo.Status = DemoStatus.Cancelled;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PostponeDemoAsync(int demoId, DateTime newDate)
        {
            var demo = await _context.Demos.FindAsync(demoId);
            if (demo == null)
                return false;

            demo.DemoDate = newDate;
            demo.Status = DemoStatus.Postponed;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}