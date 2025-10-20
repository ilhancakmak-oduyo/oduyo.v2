using MassTransit;
using Microsoft.Extensions.Logging;
using Oduyo.Domain.Messages;

namespace Oduyo.BackgroundServices.Consumers
{
    public class GenerateOfferPdfConsumer : IConsumer<GenerateOfferPdfMessage>
    {
        private readonly IPdfService _pdfService;
        private readonly IBus _bus;
        private readonly ILogger<GenerateOfferPdfConsumer> _logger;

        public GenerateOfferPdfConsumer(
            IPdfService pdfService,
            IBus bus,
            ILogger<GenerateOfferPdfConsumer> logger)
        {
            _pdfService = pdfService;
            _bus = bus;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<GenerateOfferPdfMessage> context)
        {
            var message = context.Message;

            try
            {
                // Generate PDF
                var pdf = await _pdfService.GenerateOfferPdfAsync(message.OfferId);

                // Send email with attachment
                await _bus.Publish(new SendEmailMessage
                {
                    To = message.EmailTo,
                    Subject = "Teklifiniz Hazır",
                    TemplateId = "offer-ready",
                    TemplateData = new Dictionary<string, string>
                    {
                        ["OfferId"] = message.OfferId.ToString()
                    },
                    Attachments = new List<EmailAttachment>
                    {
                        new()
                        {
                            FileName = $"Teklif_{message.OfferId}.pdf",
                            Content = pdf,
                            ContentType = "application/pdf"
                        }
                    }
                });

                _logger.LogInformation("Offer PDF generated and sent for {OfferId}", message.OfferId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate PDF for offer {OfferId}", message.OfferId);
                throw;
            }
        }
    }

    public interface IPdfService
    {
        Task<byte[]> GenerateOfferPdfAsync(int offerId);
    }
}
