namespace Oduyo.Domain.DTOs
{
    public class UpdateEmailTemplateDto
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
    }
}