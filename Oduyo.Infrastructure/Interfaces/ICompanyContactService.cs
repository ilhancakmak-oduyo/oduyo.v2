using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface ICompanyContactService
    {
        Task<CompanyContact> CreateContactAsync(CreateCompanyContactDto dto);
        Task<CompanyContact> UpdateContactAsync(int contactId, UpdateCompanyContactDto dto);
        Task<bool> DeleteContactAsync(int contactId);
        Task<CompanyContact> GetContactByIdAsync(int contactId);
        Task<List<CompanyContact>> GetCompanyContactsAsync(int companyId);
        Task<CompanyContact> GetPrimaryContactAsync(int companyId);
        Task<bool> SetPrimaryContactAsync(int companyId, int contactId);
    }
}