using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ICreditService
    {
        Task<Credit> CreateCreditAsync(CreateCreditDto dto);
        Task<bool> UseCreditAsync(int creditId, int amount);
        Task<bool> AddCreditAsync(int creditId, int amount);
        Task<Credit> GetCreditByIdAsync(int creditId);
        Task<List<Credit>> GetCompanyCreditsAsync(int companyId);
        Task<Credit> GetCompanyCreditByProductAsync(int companyId, int productId);
        Task<bool> HasSufficientCreditAsync(int companyId, int productId, int requiredAmount);
    }
}