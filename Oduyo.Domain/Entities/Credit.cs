namespace Oduyo.Domain.Entities
{
    public class Credit : EntityBase
    {
        public int CompanyId { get; set; }
        public int ProductId { get; set; }
        public int TotalCredit { get; set; }
        public int UsedCredit { get; set; }
        public int RemainingCredit { get; set; }
        public DateTime? ExpiryDate { get; set; }
    }
}
