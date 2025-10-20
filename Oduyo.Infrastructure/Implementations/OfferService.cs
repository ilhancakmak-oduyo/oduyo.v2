using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class OfferService : IOfferService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDiscountAuthorityService _discountAuthorityService;

        public OfferService(ApplicationDbContext context, IDiscountAuthorityService discountAuthorityService)
        {
            _context = context;
            _discountAuthorityService = discountAuthorityService;
        }

        public async Task<Offer> CreateOfferAsync(CreateOfferDto dto)
        {
            // İndirim yetkisi kontrolü
            // TODO: Get current user ID from auth context
            // if (dto.ManualDiscountRate > 0)
            // {
            //     var canApply = await _discountAuthorityService.CanUserApplyDiscountAsync(currentUserId, dto.ManualDiscountRate);
            //     if (!canApply)
            //         throw new InvalidOperationException("İndirim yetkiniz bu oranı aşıyor. Üst yönetim onayı gerekli.");
            // }

            var offerNo = await GenerateOfferNoAsync();

            var offer = new Offer
            {
                OfferNo = offerNo,
                CompanyId = dto.CompanyId,
                DealerId = dto.DealerId,
                OfferDate = dto.OfferDate,
                ValidUntil = dto.ValidUntil,
                CurrencyId = dto.CurrencyId,
                PriceType = dto.PriceType,
                ManualDiscount = dto.ManualDiscount,
                ManualDiscountRate = dto.ManualDiscountRate,
                Description = dto.Description,
                Status = OfferStatus.Draft
            };

            _context.Offers.Add(offer);
            await _context.SaveChangesAsync();

            // Detayları ekle
            if (dto.Details != null && dto.Details.Any())
            {
                foreach (var detailDto in dto.Details)
                {
                    var detail = new OfferDetail
                    {
                        OfferId = offer.Id,
                        ProductId = detailDto.ProductId,
                        PackageId = detailDto.PackageId,
                        ModuleId = detailDto.ModuleId,
                        UnitPrice = detailDto.UnitPrice,
                        Quantity = detailDto.Quantity,
                        DiscountAmount = detailDto.DiscountAmount,
                        DiscountRate = detailDto.DiscountRate,
                        Total = (detailDto.UnitPrice * detailDto.Quantity) - detailDto.DiscountAmount
                    };

                    _context.OfferDetails.Add(detail);
                }

                await _context.SaveChangesAsync();
            }

            // Toplam hesapla
            offer.TotalAmount = await CalculateOfferTotalAsync(offer.Id);
            await _context.SaveChangesAsync();

            return offer;
        }

        public async Task<Offer> UpdateOfferAsync(int offerId, UpdateOfferDto dto)
        {
            var offer = await _context.Offers.FindAsync(offerId);
            if (offer == null)
                throw new InvalidOperationException("Teklif bulunamadı.");

            // İndirim yetkisi kontrolü
            // TODO: Get current user ID from auth context
            // if (dto.ManualDiscountRate > offer.ManualDiscountRate)
            // {
            //     var canApply = await _discountAuthorityService.CanUserApplyDiscountAsync(currentUserId, dto.ManualDiscountRate);
            //     if (!canApply)
            //         throw new InvalidOperationException("İndirim yetkiniz bu oranı aşıyor.");
            // }

            offer.OfferDate = dto.OfferDate;
            offer.ValidUntil = dto.ValidUntil;
            offer.CurrencyId = dto.CurrencyId;
            offer.PriceType = dto.PriceType;
            offer.ManualDiscount = dto.ManualDiscount;
            offer.ManualDiscountRate = dto.ManualDiscountRate;
            offer.Description = dto.Description;
            offer.Status = dto.Status;

            await _context.SaveChangesAsync();
            return offer;
        }

        public async Task<bool> DeleteOfferAsync(int offerId)
        {
            var offer = await _context.Offers.FindAsync(offerId);
            if (offer == null)
                return false;

            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Offer> GetOfferByIdAsync(int offerId)
        {
            return await _context.Offers.FirstOrDefaultAsync(o => o.Id == offerId);
        }

        public async Task<Offer> GetOfferByOfferNoAsync(string offerNo)
        {
            return await _context.Offers.FirstOrDefaultAsync(o => o.OfferNo == offerNo);
        }

        public async Task<List<Offer>> GetAllOffersAsync()
        {
            return await _context.Offers
                .OrderByDescending(o => o.OfferDate)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetOffersByCompanyAsync(int companyId)
        {
            return await _context.Offers
                .Where(o => o.CompanyId == companyId)
                .OrderByDescending(o => o.OfferDate)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetOffersByDealerAsync(int dealerId)
        {
            return await _context.Offers
                .Where(o => o.DealerId == dealerId)
                .OrderByDescending(o => o.OfferDate)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetOffersByStatusAsync(OfferStatus status)
        {
            return await _context.Offers
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.OfferDate)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetExpiredOffersAsync()
        {
            var today = DateTime.UtcNow.Date;

            return await _context.Offers
                .Where(o => o.ValidUntil < today && o.Status == OfferStatus.Sent)
                .ToListAsync();
        }

        public async Task<bool> ApproveOfferAsync(int offerId)
        {
            var offer = await _context.Offers.FindAsync(offerId);
            if (offer == null)
                return false;

            offer.Status = OfferStatus.Approved;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectOfferAsync(int offerId, string rejectionReason)
        {
            var offer = await _context.Offers.FindAsync(offerId);
            if (offer == null)
                return false;

            offer.Status = OfferStatus.Rejected;
            offer.RejectionReason = rejectionReason;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> GenerateOfferNoAsync()
        {
            var year = DateTime.UtcNow.Year;
            var prefix = $"TKL-{year}-";

            var lastOffer = await _context.Offers
                .Where(o => o.OfferNo.StartsWith(prefix))
                .OrderByDescending(o => o.OfferNo)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (lastOffer != null)
            {
                var lastNumberStr = lastOffer.OfferNo.Replace(prefix, "");
                if (int.TryParse(lastNumberStr, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{prefix}{nextNumber:D4}";
        }

        public async Task<decimal> CalculateOfferTotalAsync(int offerId)
        {
            var details = await _context.OfferDetails
                .Where(od => od.OfferId == offerId)
                .ToListAsync();

            var subTotal = details.Sum(d => d.Total);

            var offer = await _context.Offers.FindAsync(offerId);
            if (offer != null)
            {
                offer.SubTotal = subTotal;
                offer.TotalAmount = subTotal - offer.ManualDiscount - offer.CampaignDiscount;
            }

            return offer?.TotalAmount ?? 0;
        }
    }
}
