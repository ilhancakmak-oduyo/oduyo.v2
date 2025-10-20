using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ILicenseService
    {
        Task<License> CreateLicenseAsync(CreateLicenseDto dto);
        Task<License> UpdateLicenseAsync(int licenseId, UpdateLicenseDto dto);
        Task<bool> DeleteLicenseAsync(int licenseId);
        Task<License> GetLicenseByIdAsync(int licenseId);
        Task<List<License>> GetCompanyLicensesAsync(int companyId);
        Task<List<License>> GetActiveLicensesAsync();
        Task<List<License>> GetExpiringLicensesAsync(int daysBeforeExpiry);
        Task<bool> RenewLicenseAsync(int licenseId, DateTime newEndDate);
        Task<bool> IsLicenseValidAsync(int companyId, int? productId = null);
    }
}