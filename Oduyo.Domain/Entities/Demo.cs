using Oduyo.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Oduyo.Domain.Entities
{
    public class Demo : EntityBase
    {
        public int CompanyId { get; set; }
        public DateTime DemoDate { get; set; }
        public DateTime? DemoEndTime { get; set; }
        public string Location { get; set; }
        public string ProductInterests { get; set; }
        public string Purpose { get; set; }
        public DemoStatus Status { get; set; }
        public int AssignedUserId { get; set; }

        // NEW - Phase 1 additions
        [StringLength(50)]
        public string ReferenceCode { get; set; }

        public int? ChannelId { get; set; }

        public DemoLocationType LocationType { get; set; }

        [StringLength(255)]
        public string CalendarEventId { get; set; }  // Google Calendar ID

        // Navigation properties
        public virtual Company Company { get; set; }
        public virtual User AssignedUser { get; set; }
        public virtual Channel Channel { get; set; }
    }
}