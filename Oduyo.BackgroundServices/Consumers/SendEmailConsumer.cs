using MassTransit;
using Microsoft.Extensions.Logging;
using Oduyo.Domain.Messages;

namespace Oduyo.BackgroundServices.Consumers
{
    public class SendEmailConsumer : IConsumer<SendEmailMessage>
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<SendEmailConsumer> _logger;

        public SendEmailConsumer(IEmailService emailService, ILogger<SendEmailConsumer> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SendEmailMessage> context)
        {
            var message = context.Message;

            try
            {
                if (!string.IsNullOrEmpty(message.TemplateId))
                {
                    await _emailService.SendTemplatedEmailAsync(
                        message.To,
                        message.Subject,
                        message.TemplateId,
                        message.TemplateData
                    );
                }
                else
                {
                    await _emailService.SendEmailAsync(
                        message.To,
                        message.Subject,
                        message.Body,
                        message.Attachments
                    );
                }

                _logger.LogInformation("Email sent to {To}", message.To);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}", message.To);
                throw;
            }
        }
    }

    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, List<EmailAttachment> attachments);
        Task SendTemplatedEmailAsync(string to, string subject, string templateId, Dictionary<string, string> templateData);
    }
}
