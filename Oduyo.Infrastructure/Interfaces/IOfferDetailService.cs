using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IOfferDetailService
    {
        Task<OfferDetail> AddDetailAsync(int offerId, CreateOfferDetailDto dto);
        Task<OfferDetail> UpdateDetailAsync(int detailId, CreateOfferDetailDto dto);
        Task<bool> DeleteDetailAsync(int detailId);
        Task<List<OfferDetail>> GetOfferDetailsAsync(int offerId);
        Task<OfferDetail> GetDetailByIdAsync(int detailId);
    }
}