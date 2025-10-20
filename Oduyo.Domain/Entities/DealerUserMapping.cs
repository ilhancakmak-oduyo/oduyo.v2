namespace Oduyo.Domain.Entities
{
    public class DealerUserMapping : EntityBase
    {
        public int DealerId { get; set; }
        public int UserId { get; set; }
    }
}