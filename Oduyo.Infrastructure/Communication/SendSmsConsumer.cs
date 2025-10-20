using MassTransit;
using Microsoft.Extensions.Logging;
using Oduyo.Domain.Messages;

namespace Oduyo.Infrastructure.Communication
{
    /// <summary>
    /// SMS gönderme mesajlarını consume eder
    /// </summary>
    public class SendSmsConsumer : IConsumer<SendSmsMessage>
    {
        private readonly ISmsService _smsService;
        private readonly ILogger<SendSmsConsumer> _logger;

        public SendSmsConsumer(
            ISmsService smsService,
            ILogger<SendSmsConsumer> logger)
        {
            _smsService = smsService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendSmsMessage> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation(
                    "Processing SMS message to {Phone}",
                    message.Phone);

                await _smsService.SendSmsAsync(
                    message.Phone,
                    message.Message,
                    message.EntityType ?? "Unknown",
                    message.EntityId);

                _logger.LogInformation(
                    "SMS sent successfully to {Phone}",
                    message.Phone);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error sending SMS to {Phone}: {Error}",
                    message.Phone,
                    ex.Message);

                // Retry mekanizması çalışsın diye exception'ı fırlat
                throw;
            }
        }
    }
}
