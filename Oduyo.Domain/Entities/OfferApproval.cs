namespace Oduyo.Domain.Entities
{
    public class OfferApproval : EntityBase
    {
        public int OfferId { get; set; }
        public int RequestedUserId { get; set; }
        public int ApproverUserId { get; set; }
        public bool IsApproved { get; set; }
        public string Notes { get; set; }
        public DateTime? ApprovedAt { get; set; }
    }
}