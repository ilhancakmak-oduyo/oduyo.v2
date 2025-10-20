using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductPricesController : ControllerBase
    {
        private readonly IProductPriceService _productPriceService;

        public ProductPricesController(IProductPriceService productPriceService)
        {
            _productPriceService = productPriceService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductPriceDto dto)
        {
            var price = await _productPriceService.CreatePriceAsync(dto.ProductId, dto.Price, dto.CurrencyId, dto.EffectiveFrom);
            return Ok(price);
        }

        [HttpGet("current/{productId}/{currencyId}")]
        public async Task<IActionResult> GetCurrentPrice(int productId, int currencyId)
        {
            var price = await _productPriceService.GetCurrentPriceAsync(productId, currencyId);
            if (price == null)
                return NotFound();
            return Ok(price);
        }

        [HttpGet("history/{productId}")]
        public async Task<IActionResult> GetProductPriceHistory(int productId)
        {
            var priceHistory = await _productPriceService.GetProductPriceHistoryAsync(productId);
            return Ok(priceHistory);
        }
    }

    public class CreateProductPriceDto
    {
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int CurrencyId { get; set; }
        public DateTime EffectiveFrom { get; set; }
    }
}
