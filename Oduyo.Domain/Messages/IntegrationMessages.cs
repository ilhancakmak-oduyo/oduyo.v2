namespace Oduyo.Domain.Messages
{
    public record GenerateOfferPdfMessage
    {
        public int OfferId { get; init; }
        public string EmailTo { get; init; }
    }

    public record SyncCalendarMessage
    {
        public int DemoId { get; init; }
        public string Title { get; init; }
        public DateTime StartTime { get; init; }
        public DateTime EndTime { get; init; }
        public string Location { get; init; }
        public List<string> Attendees { get; init; }
    }

    public record CreateAccountingInvoiceMessage
    {
        public int OfferId { get; init; }
        public int CompanyId { get; init; }
        public decimal TotalAmount { get; init; }
        public List<InvoiceLineItem> LineItems { get; init; }
    }

    public record InvoiceLineItem
    {
        public string Description { get; init; }
        public decimal Quantity { get; init; }
        public decimal UnitPrice { get; init; }
        public decimal Amount { get; init; }
    }

    public record SendBulkNotificationsMessage
    {
        public List<SendSmsMessage> SmsMessages { get; init; }
        public List<SendEmailMessage> EmailMessages { get; init; }
    }
}
