using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiscountAuthoritiesController : ControllerBase
    {
        private readonly IDiscountAuthorityService _discountAuthorityService;

        public DiscountAuthoritiesController(IDiscountAuthorityService discountAuthorityService)
        {
            _discountAuthorityService = discountAuthorityService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDiscountAuthorityDto dto)
        {
            var authority = await _discountAuthorityService.CreateAuthorityAsync(dto);
            return Ok(authority);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDiscountAuthorityDto dto)
        {
            var authority = await _discountAuthorityService.UpdateAuthorityAsync(id, dto);
            if (authority == null)
                return NotFound();
            return Ok(authority);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _discountAuthorityService.DeleteAuthorityAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserDiscountAuthority(int userId)
        {
            var authority = await _discountAuthorityService.GetUserDiscountAuthorityAsync(userId);
            if (authority == null)
                return NotFound();
            return Ok(authority);
        }

        [HttpGet("user/{userId}/can-apply/{discountRate}")]
        public async Task<IActionResult> CanUserApplyDiscount(int userId, decimal discountRate)
        {
            var canApply = await _discountAuthorityService.CanUserApplyDiscountAsync(userId, discountRate);
            return Ok(new { CanApply = canApply });
        }
    }
}
