namespace Oduyo.Domain.DTOs
{
    public class CreateDiscountAuthorityDto
    {
        public int UserId { get; set; }
        public decimal MaxDiscountRate { get; set; }
    }
}