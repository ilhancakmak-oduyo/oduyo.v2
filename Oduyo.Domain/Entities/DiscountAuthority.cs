namespace Oduyo.Domain.Entities
{
    public class DiscountAuthority : EntityBase
    {
        public int UserId { get; set; }
        public decimal MaxDiscountRate { get; set; }
        public bool IsActive { get; set; }
    }
}