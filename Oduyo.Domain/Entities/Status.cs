namespace Oduyo.Domain.Entities
{
    public class Status : EntityBase
    {
        public string Name { get; set; }
        public string EntityType { get; set; }
        public bool IsActive { get; set; }
    }
}
