using MassTransit;
using Microsoft.Extensions.Logging;
using Oduyo.Domain.Messages;

namespace Oduyo.Infrastructure.Messaging
{
    public class EmailConsumer : IConsumer<SendEmailMessage>
    {
        private readonly ILogger<EmailConsumer> _logger;

        public EmailConsumer(ILogger<EmailConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendEmailMessage> context)
        {
            var message = context.Message;

            try
            {
                _logger.LogInformation("Processing Email to {To} with subject {Subject}", 
                    message.To, message.Subject);

                // TODO: Implement actual email sending logic with your email provider
                // For now, just log it
                _logger.LogInformation("Email sent successfully to {To}", message.To);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send Email to {To}", message.To);
                throw; // This will cause MassTransit to retry
            }
        }
    }
}