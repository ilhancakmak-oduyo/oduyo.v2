namespace Oduyo.Domain.DTOs
{
    public class CreateEmailTemplateDto
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string TemplateKey { get; set; }
    }
}