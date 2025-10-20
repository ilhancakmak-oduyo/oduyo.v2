namespace Oduyo.Domain.DTOs
{
    public class CreateOfferApprovalDto
    {
        public int OfferId { get; set; }
        public int RequestedUserId { get; set; }
        public int ApproverUserId { get; set; }
        public string Notes { get; set; }
    }
}