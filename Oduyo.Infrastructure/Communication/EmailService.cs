using MassTransit;
using Oduyo.Domain.Messages;

namespace Oduyo.Infrastructure.Communication
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, string templateName = null, int? entityId = null);
    }

    public class EmailService : IEmailService
    {
        private readonly IBus _bus;

        public EmailService(IBus bus)
        {
            _bus = bus;
        }

        public async Task SendEmailAsync(string to, string subject, string body, string templateName = null, int? entityId = null)
        {
            var emailMessage = new SendEmailMessage
            {
                To = to,
                Subject = subject,
                Body = body,
                TemplateName = templateName,
                EntityId = entityId,
                SentAt = DateTime.UtcNow
            };

            await _bus.Publish(emailMessage);
        }
    }
}