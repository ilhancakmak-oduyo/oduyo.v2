using MassTransit;
using Microsoft.Extensions.Logging;
using Oduyo.Domain.Messages;

namespace Oduyo.Infrastructure.Communication
{
    /// <summary>
    /// Email gönderme mesajlarını consume eder
    /// </summary>
    public class SendEmailConsumer : IConsumer<SendEmailMessage>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<SendEmailConsumer> _logger;

        public SendEmailConsumer(
            IEmailService emailService,
            ILogger<SendEmailConsumer> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendEmailMessage> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation(
                    "Processing email message to {To} with subject {Subject}",
                    message.To,
                    message.Subject);

                // Email gönder (EmailService bus'a publish eder)
                await _emailService.SendEmailAsync(
                    message.To,
                    message.Subject,
                    message.Body,
                    message.TemplateName,
                    message.EntityId);

                _logger.LogInformation(
                    "Email sent successfully to {To}",
                    message.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error sending email to {To}: {Error}",
                    message.To,
                    ex.Message);

                // Retry mekanizması çalışsın diye exception'ı fırlat
                throw;
            }
        }
    }
}
