using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;

        public CompanyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Company> CreateCompanyAsync(CreateCompanyDto dto)
        {
            // ReferenceCode yoksa otomatik oluştur
            var referenceCode = string.IsNullOrEmpty(dto.ReferenceCode)
                ? await GenerateReferenceCodeAsync()
                : dto.ReferenceCode;

            // ReferenceCode benzersiz mi kontrol et
            var exists = await _context.Companies
                .AnyAsync(c => c.ReferenceCode == referenceCode);

            if (exists)
                throw new InvalidOperationException("Bu referans kodu zaten kullanılıyor.");

            var company = new Company
            {
                ChannelId = dto.ChannelId,
                DealerId = dto.DealerId,
                Title = dto.Title,
                TaxNumber = dto.TaxNumber,
                TaxOffice = dto.TaxOffice,
                Status = dto.Status,
                SupportUserId = dto.SupportUserId,
                ReferenceCode = referenceCode,
                Notes = dto.Notes
            };

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company> UpdateCompanyAsync(int companyId, UpdateCompanyDto dto)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
                throw new InvalidOperationException("Firma bulunamadı.");

            company.ChannelId = dto.ChannelId;
            company.DealerId = dto.DealerId;
            company.Title = dto.Title;
            company.TaxNumber = dto.TaxNumber;
            company.TaxOffice = dto.TaxOffice;
            company.Status = dto.Status;
            company.SupportUserId = dto.SupportUserId;
            company.Notes = dto.Notes;

            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<bool> DeleteCompanyAsync(int companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Company> GetCompanyByIdAsync(int companyId)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

        public async Task<Company> GetCompanyByReferenceCodeAsync(string referenceCode)
        {
            return await _context.Companies
                .FirstOrDefaultAsync(c => c.ReferenceCode == referenceCode);
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            return await _context.Companies
                .ToListAsync();
        }

        public async Task<List<Company>> GetCompaniesByStatusAsync(CompanyStatus status)
        {
            return await _context.Companies
                .Where(c => c.Status == status)
                .ToListAsync();
        }

        public async Task<List<Company>> GetCompaniesByChannelAsync(int channelId)
        {
            return await _context.Companies
                .Where(c => c.ChannelId == channelId)
                .ToListAsync();
        }

        public async Task<List<Company>> GetCompaniesByDealerAsync(int dealerId)
        {
            return await _context.Companies
                .Where(c => c.DealerId == dealerId)
                .ToListAsync();
        }

        public async Task<List<Company>> GetCompaniesBySupportUserAsync(int supportUserId)
        {
            return await _context.Companies
                .Where(c => c.SupportUserId == supportUserId)
                .ToListAsync();
        }

        public async Task<string> GenerateReferenceCodeAsync()
        {
            // Hash algoritması ile benzersiz kod oluştur
            var timestamp = DateTime.UtcNow.Ticks;
            var random = new Random().Next(1000, 9999);
            var hash = $"{timestamp}{random}";

            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var inputBytes = System.Text.Encoding.ASCII.GetBytes(hash);
                var hashBytes = md5.ComputeHash(inputBytes);
                var referenceCode = Convert.ToHexString(hashBytes)[..8].ToUpper();

                // Benzersiz mi kontrol et
                var exists = await _context.Companies.AnyAsync(c => c.ReferenceCode == referenceCode);

                if (exists)
                    return await GenerateReferenceCodeAsync(); // Recursive call

                return referenceCode;
            }
        }
    }
}