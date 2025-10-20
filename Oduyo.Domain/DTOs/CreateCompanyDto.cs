using Oduyo.Domain.Enums;

namespace Oduyo.Domain.DTOs
{
    public class CreateCompanyDto
    {
        public int? ChannelId { get; set; }
        public int? DealerId { get; set; }
        public string Title { get; set; }
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public string ReferenceCode { get; set; }
        public string Notes { get; set; }
        public CompanyStatus Status { get; set; }
        public int? SupportUserId { get; set; }
    }
}