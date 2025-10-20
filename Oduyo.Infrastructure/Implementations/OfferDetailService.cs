using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class OfferDetailService : IOfferDetailService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOfferService _offerService;

        public OfferDetailService(ApplicationDbContext context, IOfferService offerService)
        {
            _context = context;
            _offerService = offerService;
        }

        public async Task<OfferDetail> AddDetailAsync(int offerId, CreateOfferDetailDto dto)
        {
            var detail = new OfferDetail
            {
                OfferId = offerId,
                ProductId = dto.ProductId,
                PackageId = dto.PackageId,
                ModuleId = dto.ModuleId,
                UnitPrice = dto.UnitPrice,
                Quantity = dto.Quantity,
                DiscountAmount = dto.DiscountAmount,
                DiscountRate = dto.DiscountRate,
                Total = (dto.UnitPrice * dto.Quantity) - dto.DiscountAmount
            };

            _context.OfferDetails.Add(detail);
            await _context.SaveChangesAsync();

            // Teklif toplamını güncelle
            await _offerService.CalculateOfferTotalAsync(offerId);

            return detail;
        }

        public async Task<OfferDetail> UpdateDetailAsync(int detailId, CreateOfferDetailDto dto)
        {
            var detail = await _context.OfferDetails.FindAsync(detailId);
            if (detail == null)
                throw new InvalidOperationException("Teklif detayı bulunamadı.");

            detail.ProductId = dto.ProductId;
            detail.PackageId = dto.PackageId;
            detail.ModuleId = dto.ModuleId;
            detail.UnitPrice = dto.UnitPrice;
            detail.Quantity = dto.Quantity;
            detail.DiscountAmount = dto.DiscountAmount;
            detail.DiscountRate = dto.DiscountRate;
            detail.Total = (dto.UnitPrice * dto.Quantity) - dto.DiscountAmount;

            await _context.SaveChangesAsync();

            // Teklif toplamını güncelle
            await _offerService.CalculateOfferTotalAsync(detail.OfferId);

            return detail;
        }

        public async Task<bool> DeleteDetailAsync(int detailId)
        {
            var detail = await _context.OfferDetails.FindAsync(detailId);
            if (detail == null)
                return false;

            var offerId = detail.OfferId;

            await _context.SaveChangesAsync();

            // Teklif toplamını güncelle
            await _offerService.CalculateOfferTotalAsync(offerId);

            return true;
        }

        public async Task<List<OfferDetail>> GetOfferDetailsAsync(int offerId)
        {
            return await _context.OfferDetails
                .Where(od => od.OfferId == offerId)
                .ToListAsync();
        }

        public async Task<OfferDetail> GetDetailByIdAsync(int detailId)
        {
            return await _context.OfferDetails.FirstOrDefaultAsync(od => od.Id == detailId);
        }
    }
}