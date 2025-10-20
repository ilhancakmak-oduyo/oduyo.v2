using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IDiscountAuthorityService
    {
        Task<DiscountAuthority> CreateAuthorityAsync(CreateDiscountAuthorityDto dto);
        Task<DiscountAuthority> UpdateAuthorityAsync(int authorityId, UpdateDiscountAuthorityDto dto);
        Task<bool> DeleteAuthorityAsync(int authorityId);
        Task<DiscountAuthority> GetUserDiscountAuthorityAsync(int userId);
        Task<bool> CanUserApplyDiscountAsync(int userId, decimal discountRate);
    }
}