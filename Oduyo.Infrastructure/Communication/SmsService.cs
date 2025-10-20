using MassTransit;
using Oduyo.Domain.Messages;

namespace Oduyo.Infrastructure.Communication
{
    public interface ISmsService
    {
        Task SendSmsAsync(string phone, string message, string entityType, int? entityId = null);
    }

    public class SmsService : ISmsService
    {
        private readonly IBus _bus;

        public SmsService(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendSmsAsync(string phone, string message, string entityType, int? entityId = null)
        {
            var smsMessage = new SendSmsMessage
            {
                Phone = phone,
                Message = message,
                EntityType = entityType,
                EntityId = entityId,
                SentAt = DateTime.UtcNow
            };

            await _bus.Publish(smsMessage);
        }
    }
}