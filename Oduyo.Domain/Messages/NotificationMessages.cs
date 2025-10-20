namespace Oduyo.Domain.Messages
{
    public record SendSmsMessage
    {
        public string Phone { get; init; }
        public string Message { get; init; }
        public int? EntityId { get; init; }
        public string EntityType { get; init; }  // "Offer", "Demo", etc.
        public DateTime SentAt { get; init; }
    }

    public record SendEmailMessage
    {
        public string To { get; init; }
        public string Subject { get; init; }
        public string Body { get; init; }
        public string TemplateName { get; init; }
        public string TemplateId { get; init; }
        public int? EntityId { get; init; }
        public Dictionary<string, string> TemplateData { get; init; }
        public List<EmailAttachment> Attachments { get; init; }
        public DateTime SentAt { get; init; }
    }

    public record EmailAttachment
    {
        public string FileName { get; init; }
        public byte[] Content { get; init; }
        public string ContentType { get; init; }
    }
}
