using Microsoft.AspNetCore.Mvc;
using Oduyo.Infrastructure.Interfaces;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Enums;

namespace Oduyo.Test.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto)
        {
            var payment = await _paymentService.CreatePaymentAsync(dto);
            return Ok(payment);
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(int id, [FromBody] CompletePaymentDto dto)
        {
            var result = await _paymentService.CompletePaymentAsync(id, dto.TransactionId);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("{id}/fail")]
        public async Task<IActionResult> Fail(int id, [FromBody] FailPaymentDto dto)
        {
            var result = await _paymentService.FailPaymentAsync(id, dto.Reason);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpPost("{id}/refund")]
        public async Task<IActionResult> Refund(int id)
        {
            var result = await _paymentService.RefundPaymentAsync(id);
            if (!result)
                return BadRequest();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var payment = await _paymentService.GetPaymentByIdAsync(id);
            if (payment == null)
                return NotFound();
            return Ok(payment);
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetCompanyPayments(int companyId)
        {
            var payments = await _paymentService.GetCompanyPaymentsAsync(companyId);
            return Ok(payments);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(PaymentStatus status)
        {
            var payments = await _paymentService.GetPaymentsByStatusAsync(status);
            return Ok(payments);
        }

        [HttpGet("offer/{offerId}")]
        public async Task<IActionResult> GetByOffer(int offerId)
        {
            var payments = await _paymentService.GetPaymentsByOfferAsync(offerId);
            return Ok(payments);
        }

        [HttpGet("company/{companyId}/total")]
        public async Task<IActionResult> GetCompanyTotalPayments(int companyId)
        {
            var total = await _paymentService.GetCompanyTotalPaymentsAsync(companyId);
            return Ok(new { Total = total });
        }
    }

    public class CompletePaymentDto
    {
        public required string TransactionId { get; set; }
    }

    public class FailPaymentDto
    {
        public required string Reason { get; set; }
    }
}
