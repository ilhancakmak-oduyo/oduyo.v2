using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class CompanyContactService : ICompanyContactService
    {
        private readonly ApplicationDbContext _context;

        public CompanyContactService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyContact> CreateContactAsync(CreateCompanyContactDto dto)
        {
            // Eğer IsPrimary=true ise, diğer primary'leri false yap
            if (dto.IsPrimary)
            {
                var existingPrimary = await _context.CompanyContacts
                    .Where(cc => cc.CompanyId == dto.CompanyId && cc.IsPrimary)
                    .ToListAsync();

                foreach (var contact in existingPrimary)
                {
                    contact.IsPrimary = false;
                }
            }

            var companyContact = new CompanyContact
            {
                CompanyId = dto.CompanyId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Phone = dto.Phone,
                Email = dto.Email,
                IsPrimary = dto.IsPrimary
            };

            _context.CompanyContacts.Add(companyContact);
            await _context.SaveChangesAsync();
            return companyContact;
        }

        public async Task<CompanyContact> UpdateContactAsync(int contactId, UpdateCompanyContactDto dto)
        {
            var contact = await _context.CompanyContacts.FindAsync(contactId);
            if (contact == null)
                throw new InvalidOperationException("İletişim kişisi bulunamadı.");

            // Eğer IsPrimary=true yapılıyorsa, diğer primary'leri false yap
            if (dto.IsPrimary && !contact.IsPrimary)
            {
                var existingPrimary = await _context.CompanyContacts
                    .Where(cc => cc.CompanyId == contact.CompanyId && cc.IsPrimary && cc.Id != contactId)
                    .ToListAsync();

                foreach (var c in existingPrimary)
                {
                    c.IsPrimary = false;
                }
            }

            contact.FirstName = dto.FirstName;
            contact.LastName = dto.LastName;
            contact.Phone = dto.Phone;
            contact.Email = dto.Email;
            contact.IsPrimary = dto.IsPrimary;

            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<bool> DeleteContactAsync(int contactId)
        {
            var contact = await _context.CompanyContacts.FindAsync(contactId);
            if (contact == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<CompanyContact> GetContactByIdAsync(int contactId)
        {
            return await _context.CompanyContacts
                .FirstOrDefaultAsync(cc => cc.Id == contactId);
        }

        public async Task<List<CompanyContact>> GetCompanyContactsAsync(int companyId)
        {
            return await _context.CompanyContacts
                .Where(cc => cc.CompanyId == companyId)
                .OrderByDescending(cc => cc.IsPrimary)
                .ToListAsync();
        }

        public async Task<CompanyContact> GetPrimaryContactAsync(int companyId)
        {
            return await _context.CompanyContacts
                .FirstOrDefaultAsync(cc => cc.CompanyId == companyId && cc.IsPrimary);
        }

        public async Task<bool> SetPrimaryContactAsync(int companyId, int contactId)
        {
            var contact = await _context.CompanyContacts
                .FirstOrDefaultAsync(cc => cc.Id == contactId && cc.CompanyId == companyId);

            if (contact == null)
                return false;

            // Diğer primary'leri false yap
            var existingPrimary = await _context.CompanyContacts
                .Where(cc => cc.CompanyId == companyId && cc.IsPrimary && cc.Id != contactId)
                .ToListAsync();

            foreach (var c in existingPrimary)
            {
                c.IsPrimary = false;
            }

            // Seçili contact'ı primary yap
            contact.IsPrimary = true;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}