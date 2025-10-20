using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IOfferService
    {
        Task<Offer> CreateOfferAsync(CreateOfferDto dto);
        Task<Offer> UpdateOfferAsync(int offerId, UpdateOfferDto dto);
        Task<bool> DeleteOfferAsync(int offerId);
        Task<Offer> GetOfferByIdAsync(int offerId);
        Task<Offer> GetOfferByOfferNoAsync(string offerNo);
        Task<List<Offer>> GetAllOffersAsync();
        Task<List<Offer>> GetOffersByCompanyAsync(int companyId);
        Task<List<Offer>> GetOffersByDealerAsync(int dealerId);
        Task<List<Offer>> GetOffersByStatusAsync(OfferStatus status);
        Task<List<Offer>> GetExpiredOffersAsync();
        Task<bool> ApproveOfferAsync(int offerId);
        Task<bool> RejectOfferAsync(int offerId, string rejectionReason);
        Task<string> GenerateOfferNoAsync();
        Task<decimal> CalculateOfferTotalAsync(int offerId);
    }
}