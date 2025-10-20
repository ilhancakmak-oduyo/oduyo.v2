using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IReferenceCodeService
    {
        Task<string> GenerateReferenceCodeAsync(string prefix);
        Task<ReferenceCodeSequence> GetSequenceAsync(string prefix);
    }
}