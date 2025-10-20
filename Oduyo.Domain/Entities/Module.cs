namespace Oduyo.Domain.Entities
{
    public class Module : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; }
    }
}