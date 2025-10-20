namespace Oduyo.Domain.Entities
{
    public class Permission : EntityBase
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Module { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}