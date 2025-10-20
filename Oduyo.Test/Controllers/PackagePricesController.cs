using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackagePricesController : ControllerBase
    {
        private readonly IPackagePriceService _packagePriceService;

        public PackagePricesController(IPackagePriceService packagePriceService)
        {
            _packagePriceService = packagePriceService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePackagePriceDto dto)
        {
            var price = await _packagePriceService.CreatePriceAsync(dto.PackageId, dto.Price, dto.CurrencyId, dto.EffectiveFrom);
            return Ok(price);
        }

        [HttpGet("current/{packageId}/{currencyId}")]
        public async Task<IActionResult> GetCurrentPrice(int packageId, int currencyId)
        {
            var price = await _packagePriceService.GetCurrentPriceAsync(packageId, currencyId);
            if (price == null)
                return NotFound();
            return Ok(price);
        }

        [HttpGet("history/{packageId}")]
        public async Task<IActionResult> GetPackagePriceHistory(int packageId)
        {
            var priceHistory = await _packagePriceService.GetPackagePriceHistoryAsync(packageId);
            return Ok(priceHistory);
        }
    }

    public class CreatePackagePriceDto
    {
        public int PackageId { get; set; }
        public decimal Price { get; set; }
        public int CurrencyId { get; set; }
        public DateTime EffectiveFrom { get; set; }
    }
}
