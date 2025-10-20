using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditsController : ControllerBase
    {
        private readonly ICreditService _creditService;

        public CreditsController(ICreditService creditService)
        {
            _creditService = creditService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCreditDto dto)
        {
            var credit = await _creditService.CreateCreditAsync(dto);
            return Ok(credit);
        }

        [HttpPost("{id}/use")]
        public async Task<IActionResult> UseCredit(int id, [FromBody] UseCreditDto dto)
        {
            var result = await _creditService.UseCreditAsync(id, dto.Amount);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("{id}/add")]
        public async Task<IActionResult> AddCredit(int id, [FromBody] AddCreditDto dto)
        {
            var result = await _creditService.AddCreditAsync(id, dto.Amount);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var credit = await _creditService.GetCreditByIdAsync(id);
            if (credit == null)
                return NotFound();
            return Ok(credit);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetCompanyCredits(int companyId)
        {
            var credits = await _creditService.GetCompanyCreditsAsync(companyId);
            return Ok(credits);
        }

        [HttpGet("company/{companyId}/product/{productId}")]
        public async Task<IActionResult> GetCompanyCreditByProduct(int companyId, int productId)
        {
            var credit = await _creditService.GetCompanyCreditByProductAsync(companyId, productId);
            if (credit == null)
                return NotFound();
            return Ok(credit);
        }

        [HttpGet("company/{companyId}/product/{productId}/has-sufficient/{requiredAmount}")]
        public async Task<IActionResult> HasSufficientCredit(int companyId, int productId, int requiredAmount)
        {
            var hasSufficient = await _creditService.HasSufficientCreditAsync(companyId, productId, requiredAmount);
            return Ok(new { HasSufficient = hasSufficient });
        }
    }

    public class UseCreditDto
    {
        public int Amount { get; set; }
    }

    public class AddCreditDto
    {
        public int Amount { get; set; }
    }
}
