using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ApplicationDbContext _context;

        public CurrencyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Currency> CreateCurrencyAsync(CreateCurrencyDto dto)
        {
            var currency = new Currency
            {
                Code = dto.Code,
                Name = dto.Name,
                Symbol = dto.Symbol,
                IsActive = true
            };

            _context.Currencies.Add(currency);
            await _context.SaveChangesAsync();
            return currency;
        }

        public async Task<Currency> UpdateCurrencyAsync(int currencyId, UpdateCurrencyDto dto)
        {
            var currency = await _context.Currencies.FindAsync(currencyId);
            if (currency == null)
                throw new InvalidOperationException("Para birimi bulunamadı.");

            currency.Name = dto.Name;
            currency.Symbol = dto.Symbol;
            currency.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return currency;
        }

        public async Task<bool> DeleteCurrencyAsync(int currencyId)
        {
            var currency = await _context.Currencies.FindAsync(currencyId);
            if (currency == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Currency> GetCurrencyByIdAsync(int currencyId)
        {
            return await _context.Currencies.FirstOrDefaultAsync(c => c.Id == currencyId);
        }

        public async Task<Currency> GetCurrencyByCodeAsync(string code)
        {
            return await _context.Currencies.FirstOrDefaultAsync(c => c.Code == code);
        }

        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            return await _context.Currencies.ToListAsync();
        }

        public async Task<List<Currency>> GetActiveCurrenciesAsync()
        {
            return await _context.Currencies.Where(c => c.IsActive).ToListAsync();
        }
    }
}