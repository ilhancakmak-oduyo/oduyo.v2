namespace Oduyo.Domain.Entities
{
    public class OfferRevision : EntityBase
    {
        public int OfferId { get; set; }
        public int RevisionNo { get; set; }
        public string Changes { get; set; }
        public string Reason { get; set; }
    }
}