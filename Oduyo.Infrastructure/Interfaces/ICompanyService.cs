using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ICompanyService
    {
        Task<Company> CreateCompanyAsync(CreateCompanyDto dto);
        Task<Company> UpdateCompanyAsync(int companyId, UpdateCompanyDto dto);
        Task<bool> DeleteCompanyAsync(int companyId);
        Task<Company> GetCompanyByIdAsync(int companyId);
        Task<Company> GetCompanyByReferenceCodeAsync(string referenceCode);
        Task<List<Company>> GetAllCompaniesAsync();
        Task<List<Company>> GetCompaniesByStatusAsync(CompanyStatus status);
        Task<List<Company>> GetCompaniesByChannelAsync(int channelId);
        Task<List<Company>> GetCompaniesByDealerAsync(int dealerId);
        Task<List<Company>> GetCompaniesBySupportUserAsync(int supportUserId);
        Task<string> GenerateReferenceCodeAsync();
    }
}