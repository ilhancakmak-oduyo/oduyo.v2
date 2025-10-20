using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class OfferRevisionService : IOfferRevisionService
    {
        private readonly ApplicationDbContext _context;

        public OfferRevisionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OfferRevision> CreateRevisionAsync(CreateOfferRevisionDto dto)
        {
            var revisionCount = await GetRevisionCountAsync(dto.OfferId);

            var revision = new OfferRevision
            {
                OfferId = dto.OfferId,
                RevisionNo = revisionCount + 1,
                Changes = dto.Changes,
                Reason = dto.Reason
            };

            _context.OfferRevisions.Add(revision);
            await _context.SaveChangesAsync();
            return revision;
        }

        public async Task<List<OfferRevision>> GetOfferRevisionsAsync(int offerId)
        {
            return await _context.OfferRevisions
                .Where(or => or.OfferId == offerId)
                .OrderBy(or => or.RevisionNo)
                .ToListAsync();
        }

        public async Task<int> GetRevisionCountAsync(int offerId)
        {
            return await _context.OfferRevisions
                .Where(or => or.OfferId == offerId)
                .CountAsync();
        }
    }
}