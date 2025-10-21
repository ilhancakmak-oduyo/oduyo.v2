using MassTransit;
using Microsoft.Extensions.Logging;
using Oduyo.Domain.Messages;

namespace Oduyo.Infrastructure.Messaging
{
    public class SmsConsumer : IConsumer<SendSmsMessage>
    {
        private readonly ILogger<SmsConsumer> _logger;

        public SmsConsumer(ILogger<SmsConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendSmsMessage> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation("Processing SMS to {Phone} for {EntityType} {EntityId}",
                    message.Phone, message.EntityType, message.EntityId);

                // TODO: Implement actual SMS sending logic with your SMS provider
                // For now, just log it
                _logger.LogInformation("SMS sent successfully: {Message}", message.Message);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send SMS to {Phone}", message.Phone);
                throw; // This will cause MassTransit to retry
            }
        }
    }
}