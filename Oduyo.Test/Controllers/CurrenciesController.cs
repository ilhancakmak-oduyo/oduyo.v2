using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCurrencyDto dto)
        {
            var currency = await _currencyService.CreateCurrencyAsync(dto);
            return Ok(currency);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCurrencyDto dto)
        {
            var currency = await _currencyService.UpdateCurrencyAsync(id, dto);
            if (currency == null)
                return NotFound();
            return Ok(currency);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _currencyService.DeleteCurrencyAsync(id);
            if (!result)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var currency = await _currencyService.GetCurrencyByIdAsync(id);
            if (currency == null)
                return NotFound();
            return Ok(currency);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currencies = await _currencyService.GetAllCurrenciesAsync();
            return Ok(currencies);
        }

        [HttpGet("code/{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var currency = await _currencyService.GetCurrencyByCodeAsync(code);
            if (currency == null)
                return NotFound();
            return Ok(currency);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var currencies = await _currencyService.GetActiveCurrenciesAsync();
            return Ok(currencies);
        }
    }
}
