using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Constants;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Messages;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class LicenseService : ILicenseService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILicenseHistoryService _licenseHistoryService;
        private readonly IBus _bus;
        private readonly ILogger<LicenseService> _logger;

        public LicenseService(
            ApplicationDbContext context,
            ILicenseHistoryService licenseHistoryService,
            IBus bus,
            ILogger<LicenseService> logger)
        {
            _context = context;
            _licenseHistoryService = licenseHistoryService;
            _bus = bus;
            _logger = logger;
        }

        public async Task<License> CreateLicenseAsync(CreateLicenseDto dto)
        {
            var license = new License
            {
                CompanyId = dto.CompanyId,
                ProductId = dto.ProductId,
                PackageId = dto.PackageId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Period = dto.Period,
                IsActive = true,
                IsTrial = dto.IsTrialPeriod,
                TrialDays = dto.IsTrialPeriod ? (int?)(dto.EndDate - dto.StartDate).TotalDays : null,
                GracePeriodDays = dto.IsTrialPeriod ? 0 : 7
            };

            _context.Licenses.Add(license);
            await _context.SaveChangesAsync();

            // Geçmiş kaydı oluştur
            await _licenseHistoryService.CreateHistoryAsync(license.Id, "Lisans oluşturuldu");

            return license;
        }

        // NEW - Phase 5: Synchronize license end date
        public async Task<License> SynchronizeLicenseEndDateAsync(
            int companyId,
            int productId,
            int? packageId,
            int createdBy)
        {
            // Find company's master license (latest end date)
            var masterLicense = await _context.Licenses
                .Where(l => l.CompanyId == companyId && l.IsActive)
                .OrderByDescending(l => l.EndDate)
                .FirstOrDefaultAsync();

            DateTime startDate = DateTime.UtcNow;
            DateTime endDate;

            if (masterLicense != null)
            {
                endDate = masterLicense.EndDate;
                _logger.LogInformation(
                    "Synchronizing license for company {CompanyId} to master end date {EndDate}",
                    companyId, endDate
                );
            }
            else
            {
                endDate = startDate.AddYears(1);
            }

            var license = new License
            {
                CompanyId = companyId,
                ProductId = productId,
                PackageId = packageId,
                StartDate = startDate,
                EndDate = endDate,
                Period = Domain.Enums.LicensePeriod.Yearly,
                IsActive = true,
                IsTrial = false,
                GracePeriodDays = 7
            };

            _context.Licenses.Add(license);

            var history = new LicenseHistory
            {
                LicenseId = license.Id,
                Action = "Created",
                NewEndDate = endDate,
                Notes = masterLicense != null
                    ? $"Synchronized to master license #{masterLicense.Id}"
                    : "Initial license"
            };
            _context.LicenseHistories.Add(history);

            await _context.SaveChangesAsync();
            return license;
        }

        // NEW - Phase 5: Create trial license
        public async Task<License> CreateTrialLicenseAsync(
            int companyId,
            int productId,
            int? packageId,
            int trialDays,
            int createdBy)
        {
            var license = new License
            {
                CompanyId = companyId,
                ProductId = productId,
                PackageId = packageId,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(trialDays),
                Period = Domain.Enums.LicensePeriod.Monthly,
                IsActive = true,
                IsTrial = true,
                TrialDays = trialDays,
                GracePeriodDays = 0
            };

            _context.Licenses.Add(license);
            await _context.SaveChangesAsync();

            var company = await _context.Companies.FindAsync(companyId);
            var product = await _context.Products.FindAsync(productId);

            await _bus.Publish(new SendEmailMessage
            {
                To = company.Email,
                Subject = "Deneme Lisansınız Aktif",
                TemplateId = "trial-activated",
                TemplateData = new Dictionary<string, string>
                {
                    ["TrialDays"] = trialDays.ToString(),
                    ["ProductName"] = product?.Name ?? "Ürün"
                }
            });

            _logger.LogInformation(
                "Trial license created for company {CompanyId}, {TrialDays} days",
                companyId, trialDays
            );

            return license;
        }

        // NEW - Phase 5: Validate license with grace period
        public async Task<LicenseValidationResult> ValidateLicenseAsync(int licenseId)
        {
            var license = await _context.Licenses
                .Include(l => l.Company)
                .Include(l => l.Product)
                .FirstOrDefaultAsync(l => l.Id == licenseId);

            if (license == null || !license.IsActive)
            {
                return new LicenseValidationResult
                {
                    IsValid = false,
                    Reason = "License not found or inactive"
                };
            }

            var now = DateTime.UtcNow;

            // Normal period
            if (now <= license.EndDate)
            {
                return new LicenseValidationResult
                {
                    IsValid = true,
                    DaysRemaining = (license.EndDate - now).Days
                };
            }

            // Grace period
            if (license.GracePeriodStartDate.HasValue)
            {
                var graceEndDate = license.GracePeriodStartDate.Value.AddDays(license.GracePeriodDays);

                if (now <= graceEndDate)
                {
                    return new LicenseValidationResult
                    {
                        IsValid = true,
                        IsInGracePeriod = true,
                        DaysRemaining = (graceEndDate - now).Days,
                        Message = "License is in grace period"
                    };
                }
            }

            return new LicenseValidationResult
            {
                IsValid = false,
                Reason = "License expired",
                ExpiredAt = license.GracePeriodStartDate?.AddDays(license.GracePeriodDays) ?? license.EndDate
            };
        }

        public async Task<License> UpdateLicenseAsync(int licenseId, UpdateLicenseDto dto)
        {
            var license = await _context.Licenses.FindAsync(licenseId);
            if (license == null)
                throw new InvalidOperationException("Lisans bulunamadı.");

            license.StartDate = dto.StartDate;
            license.EndDate = dto.EndDate;
            license.Period = dto.Period;
            license.IsActive = dto.IsActive;
            license.IsTrial = dto.IsTrialPeriod;

            await _context.SaveChangesAsync();

            // Geçmiş kaydı oluştur
            await _licenseHistoryService.CreateHistoryAsync(license.Id, "Lisans güncellendi");

            return license;
        }

        public async Task<bool> DeleteLicenseAsync(int licenseId)
        {
            var license = await _context.Licenses.FindAsync(licenseId);
            if (license == null)
                return false;

            await _context.SaveChangesAsync();

            // Geçmiş kaydı oluştur
            await _licenseHistoryService.CreateHistoryAsync(license.Id, "Lisans silindi");

            return true;
        }

        public async Task<License> GetLicenseByIdAsync(int licenseId)
        {
            return await _context.Licenses.FirstOrDefaultAsync(l => l.Id == licenseId);
        }

        public async Task<List<License>> GetCompanyLicensesAsync(int companyId)
        {
            return await _context.Licenses
                .Where(l => l.CompanyId == companyId)
                .OrderByDescending(l => l.StartDate)
                .ToListAsync();
        }

        public async Task<List<License>> GetActiveLicensesAsync()
        {
            var now = DateTime.UtcNow;
            return await _context.Licenses
                .Where(l => l.IsActive && l.StartDate <= now && l.EndDate >= now)
                .ToListAsync();
        }

        public async Task<List<License>> GetExpiringLicensesAsync(int daysBeforeExpiry)
        {
            var now = DateTime.UtcNow;
            var expiryDate = now.AddDays(daysBeforeExpiry);

            return await _context.Licenses
                .Where(l => l.IsActive &&
                           l.EndDate >= now &&
                           l.EndDate <= expiryDate)
                .ToListAsync();
        }

        public async Task<bool> RenewLicenseAsync(int licenseId, DateTime newEndDate)
        {
            var license = await _context.Licenses.FindAsync(licenseId);
            if (license == null)
                return false;

            license.EndDate = newEndDate;

            await _context.SaveChangesAsync();

            // Geçmiş kaydı oluştur
            await _licenseHistoryService.CreateHistoryAsync(
                license.Id,
                $"Lisans yenilendi - Yeni bitiş tarihi: {newEndDate:dd.MM.yyyy}");

            return true;
        }

        public async Task<bool> IsLicenseValidAsync(int companyId, int? productId = null)
        {
            var now = DateTime.UtcNow;

            var query = _context.Licenses
                .Where(l => l.CompanyId == companyId &&
                           l.IsActive &&
                           l.StartDate <= now &&
                           l.EndDate >= now);

            if (productId.HasValue)
                query = query.Where(l => l.ProductId == productId.Value);

            return await query.AnyAsync();
        }
    }

    // NEW - Phase 5: License validation result model
    public class LicenseValidationResult
    {
        public bool IsValid { get; set; }
        public bool IsInGracePeriod { get; set; }
        public int DaysRemaining { get; set; }
        public string Message { get; set; }
        public string Reason { get; set; }
        public DateTime? ExpiredAt { get; set; }
    }
}