using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ICurrencyService
    {
        Task<Currency> CreateCurrencyAsync(CreateCurrencyDto dto);
        Task<Currency> UpdateCurrencyAsync(int currencyId, UpdateCurrencyDto dto);
        Task<bool> DeleteCurrencyAsync(int currencyId);
        Task<Currency> GetCurrencyByIdAsync(int currencyId);
        Task<Currency> GetCurrencyByCodeAsync(string code);
        Task<List<Currency>> GetAllCurrenciesAsync();
        Task<List<Currency>> GetActiveCurrenciesAsync();
    }
}