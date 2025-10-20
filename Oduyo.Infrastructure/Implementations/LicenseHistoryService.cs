using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class LicenseHistoryService : ILicenseHistoryService
    {
        private readonly ApplicationDbContext _context;

        public LicenseHistoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<LicenseHistory> CreateHistoryAsync(int licenseId, string action)
        {
            var license = await _context.Licenses.FindAsync(licenseId);
            if (license == null)
                throw new InvalidOperationException("Lisans bulunamadı.");

            var history = new LicenseHistory
            {
                LicenseId = licenseId,
                StartDate = license.StartDate,
                EndDate = license.EndDate,
                Action = action
            };

            _context.LicenseHistories.Add(history);
            await _context.SaveChangesAsync();

            return history;
        }

        public async Task<List<LicenseHistory>> GetLicenseHistoryAsync(int licenseId)
        {
            return await _context.LicenseHistories
                .Where(lh => lh.LicenseId == licenseId)
                .OrderByDescending(lh => lh.CreatedAt)
                .ToListAsync();
        }
    }
}