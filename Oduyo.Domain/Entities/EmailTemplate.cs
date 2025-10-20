namespace Oduyo.Domain.Entities
{
    public class EmailTemplate : EntityBase
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string TemplateKey { get; set; }
        public bool IsActive { get; set; }
    }
}
