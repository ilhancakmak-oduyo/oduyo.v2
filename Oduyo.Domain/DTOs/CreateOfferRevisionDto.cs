namespace Oduyo.Domain.DTOs
{
    public class CreateOfferRevisionDto
    {
        public int OfferId { get; set; }
        public string Changes { get; set; }
        public string Reason { get; set; }
    }
}