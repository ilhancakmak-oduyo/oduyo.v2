namespace Oduyo.Domain.DTOs
{
    public class CreateCreditDto
    {
        public int CompanyId { get; set; }
        public int ProductId { get; set; }
        public int TotalCredit { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}