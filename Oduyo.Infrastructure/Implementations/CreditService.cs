using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class CreditService : ICreditService
    {
        private readonly ApplicationDbContext _context;

        public CreditService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Credit> CreateCreditAsync(CreateCreditDto dto)
        {
            var credit = new Credit
            {
                CompanyId = dto.CompanyId,
                ProductId = dto.ProductId,
                TotalCredit = dto.TotalCredit,
                UsedCredit = 0,
                RemainingCredit = dto.TotalCredit,
                ExpiryDate = dto.ExpiryDate
            };

            _context.Credits.Add(credit);
            await _context.SaveChangesAsync();

            return credit;
        }

        public async Task<bool> UseCreditAsync(int creditId, int amount)
        {
            var credit = await _context.Credits.FindAsync(creditId);
            if (credit == null)
                return false;

            if (credit.RemainingCredit < amount)
                throw new InvalidOperationException("Yetersiz kredi.");

            credit.UsedCredit += amount;
            credit.RemainingCredit -= amount;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddCreditAsync(int creditId, int amount)
        {
            var credit = await _context.Credits.FindAsync(creditId);
            if (credit == null)
                return false;

            credit.TotalCredit += amount;
            credit.RemainingCredit += amount;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Credit> GetCreditByIdAsync(int creditId)
        {
            return await _context.Credits.FirstOrDefaultAsync(c => c.Id == creditId);
        }

        public async Task<List<Credit>> GetCompanyCreditsAsync(int companyId)
        {
            return await _context.Credits
                .Where(c => c.CompanyId == companyId)
                .ToListAsync();
        }

        public async Task<Credit> GetCompanyCreditByProductAsync(int companyId, int productId)
        {
            return await _context.Credits
                .FirstOrDefaultAsync(c => c.CompanyId == companyId && c.ProductId == productId);
        }

        public async Task<bool> HasSufficientCreditAsync(int companyId, int productId, int requiredAmount)
        {
            var credit = await GetCompanyCreditByProductAsync(companyId, productId);

            if (credit == null)
                return false;

            // Kredi süresi dolmuş mu kontrol et
            if (credit.ExpiryDate.HasValue && credit.ExpiryDate.Value < DateTime.UtcNow)
                return false;

            return credit.RemainingCredit >= requiredAmount;
        }
    }
}