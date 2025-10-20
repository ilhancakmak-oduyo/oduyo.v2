using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class DealerService : IDealerService
    {
        private readonly ApplicationDbContext _context;

        public DealerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dealer> CreateDealerAsync(CreateDealerDto dto)
        {
            var dealer = new Dealer
            {
                Title = dto.Title,
                TaxNumber = dto.TaxNumber,
                TaxOffice = dto.TaxOffice,
                Phone = dto.Phone,
                Email = dto.Email,
                Address = dto.Address,
                IsActive = true
            };

            _context.Dealers.Add(dealer);
            await _context.SaveChangesAsync();
            return dealer;
        }

        public async Task<Dealer> UpdateDealerAsync(int dealerId, UpdateDealerDto dto)
        {
            var dealer = await _context.Dealers.FindAsync(dealerId);
            if (dealer == null)
                throw new InvalidOperationException("Bayi bulunamadı.");

            dealer.Title = dto.Title;
            dealer.TaxNumber = dto.TaxNumber;
            dealer.TaxOffice = dto.TaxOffice;
            dealer.Phone = dto.Phone;
            dealer.Email = dto.Email;
            dealer.Address = dto.Address;
            dealer.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return dealer;
        }

        public async Task<bool> DeleteDealerAsync(int dealerId)
        {
            var dealer = await _context.Dealers.FindAsync(dealerId);
            if (dealer == null)
                return false;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Dealer> GetDealerByIdAsync(int dealerId)
        {
            return await _context.Dealers
                .FirstOrDefaultAsync(d => d.Id == dealerId);
        }

        public async Task<List<Dealer>> GetAllDealersAsync()
        {
            return await _context.Dealers.ToListAsync();
        }

        public async Task<List<Dealer>> GetActiveDealersAsync()
        {
            return await _context.Dealers
                .Where(d => d.IsActive)
                .ToListAsync();
        }
    }
}