using MassTransit;
using Microsoft.Extensions.Logging;
using Oduyo.Domain.Messages;

namespace Oduyo.BackgroundServices.Consumers
{
    public class SendSmsConsumer : IConsumer<SendSmsMessage>
    {
        private readonly ISmsService _smsService;
        private readonly ILogger<SendSmsConsumer> _logger;

        public SendSmsConsumer(ISmsService smsService, ILogger<SendSmsConsumer> logger)
        {
            _smsService = smsService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendSmsMessage> context)
        {
            var message = context.Message;

            try
            {
                await _smsService.SendSmsAsync(message.Phone, message.Message);

                _logger.LogInformation(
                    "SMS sent to {Phone} for {EntityType} {EntityId}",
                    message.Phone, message.EntityType, message.RelatedEntityId
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SMS to {Phone}", message.Phone);
                throw;  // MassTransit will retry
            }
        }
    }

    public interface ISmsService
    {
        Task SendSmsAsync(string phone, string message);
    }
}
