using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ILicenseHistoryService
    {
        Task<LicenseHistory> CreateHistoryAsync(int licenseId, string action);
        Task<List<LicenseHistory>> GetLicenseHistoryAsync(int licenseId);
    }
}