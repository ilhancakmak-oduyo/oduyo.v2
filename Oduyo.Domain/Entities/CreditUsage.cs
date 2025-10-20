using System.ComponentModel.DataAnnotations;

namespace Oduyo.Domain.Entities
{
    public class CreditUsage : EntityBase
    {
        public int CreditId { get; set; }
        public int CompanyId { get; set; }
        public int Amount { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime UsedAt { get; set; }

        // Navigation properties
        public virtual Credit Credit { get; set; }
        public virtual Company Company { get; set; }
    }
}
