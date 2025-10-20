namespace Oduyo.Domain.DTOs
{
    public class CreateDemoDto
    {
        public int CompanyId { get; set; }
        public DateTime DemoDate { get; set; }
        public string Location { get; set; }
        public string ProductInterests { get; set; }
        public string Purpose { get; set; }
        public int AssignedUserId { get; set; }
    }
}