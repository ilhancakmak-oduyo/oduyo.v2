namespace Oduyo.Domain.Entities
{
    public class Tenant : EntityBase
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string ConnectionString { get; set; }
        public bool IsActive { get; set; }
    }
}