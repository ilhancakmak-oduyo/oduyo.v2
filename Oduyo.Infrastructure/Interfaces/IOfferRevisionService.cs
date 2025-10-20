using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IOfferRevisionService
    {
        Task<OfferRevision> CreateRevisionAsync(CreateOfferRevisionDto dto);
        Task<List<OfferRevision>> GetOfferRevisionsAsync(int offerId);
        Task<int> GetRevisionCountAsync(int offerId);
    }
}