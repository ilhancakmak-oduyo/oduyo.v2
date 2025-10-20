using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.Entities;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class ReferenceCodeService : IReferenceCodeService
    {
        private readonly ApplicationDbContext _context;

        public ReferenceCodeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateReferenceCodeAsync(string prefix)
        {
            var sequence = await _context.ReferenceCodeSequences
                .FirstOrDefaultAsync(r => r.Prefix == prefix);

            if (sequence == null)
            {
                sequence = new ReferenceCodeSequence
                {
                    Prefix = prefix,
                    CurrentValue = 1
                };
                _context.ReferenceCodeSequences.Add(sequence);
            }
            else
            {
                sequence.CurrentValue++;
            }

            await _context.SaveChangesAsync();

            return $"{prefix}{sequence.CurrentValue:D6}";
        }

        public async Task<ReferenceCodeSequence> GetSequenceAsync(string prefix)
        {
            return await _context.ReferenceCodeSequences
                .FirstOrDefaultAsync(r => r.Prefix == prefix);
        }
    }
}