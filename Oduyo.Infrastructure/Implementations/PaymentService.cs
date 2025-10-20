using Microsoft.EntityFrameworkCore;
using Oduyo.DataAccess.DataContexts;
using Oduyo.Domain.DTOs;
using Oduyo.Domain.Entities;
using Oduyo.Domain.Enums;
using Oduyo.Infrastructure.Interfaces;

namespace Oduyo.Infrastructure.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _context;

        public PaymentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Payment> CreatePaymentAsync(CreatePaymentDto dto)
        {
            var payment = new Payment
            {
                CompanyId = dto.CompanyId,
                OfferId = dto.OfferId,
                Amount = dto.Amount,
                CurrencyId = dto.CurrencyId,
                Status = PaymentStatus.Pending,
                VirtualPOSId = dto.VirtualPOSId
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment;
        }

        public async Task<bool> CompletePaymentAsync(int paymentId, string transactionId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
                return false;

            payment.Status = PaymentStatus.Completed;
            payment.TransactionId = transactionId;
            payment.CompletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> FailPaymentAsync(int paymentId, string reason)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null)
                return false;

            payment.Status = PaymentStatus.Failed;
            payment.TransactionId = reason; // Hata sebebini TransactionId'ye kaydedebiliriz

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RefundPaymentAsync(int paymentId)
        {
            var payment = await _context.Payments.FindAsync(paymentId);
            if (payment == null || payment.Status != PaymentStatus.Completed)
                return false;

            payment.Status = PaymentStatus.Refunded;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Payment> GetPaymentByIdAsync(int paymentId)
        {
            return await _context.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        public async Task<List<Payment>> GetCompanyPaymentsAsync(int companyId)
        {
            return await _context.Payments
                .Where(p => p.CompanyId == companyId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            return await _context.Payments
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Payment>> GetPaymentsByOfferAsync(int offerId)
        {
            return await _context.Payments
                .Where(p => p.OfferId == offerId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<decimal> GetCompanyTotalPaymentsAsync(int companyId)
        {
            return await _context.Payments
                .Where(p => p.CompanyId == companyId && p.Status == PaymentStatus.Completed)
                .SumAsync(p => p.Amount);
        }
    }
}