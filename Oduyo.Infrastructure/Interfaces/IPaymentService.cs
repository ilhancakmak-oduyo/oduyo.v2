using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;

namespace Oduyo.Infrastructure.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> CreatePaymentAsync(CreatePaymentDto dto);
        Task<bool> CompletePaymentAsync(int paymentId, string transactionId);
        Task<bool> FailPaymentAsync(int paymentId, string reason);
        Task<bool> RefundPaymentAsync(int paymentId);
        Task<Payment> GetPaymentByIdAsync(int paymentId);
        Task<List<Payment>> GetCompanyPaymentsAsync(int companyId);
        Task<List<Payment>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<List<Payment>> GetPaymentsByOfferAsync(int offerId);
        Task<decimal> GetCompanyTotalPaymentsAsync(int companyId);
    }
}