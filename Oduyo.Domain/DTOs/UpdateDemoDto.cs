using Oduyo.Domain.Enums;

namespace Oduyo.Domain.DTOs
{
    public class UpdateDemoDto
    {
        public DateTime DemoDate { get; set; }
        public string Location { get; set; }
        public string ProductInterests { get; set; }
        public string Purpose { get; set; }
        public DemoStatus Status { get; set; }
        public int AssignedUserId { get; set; }
    }
}