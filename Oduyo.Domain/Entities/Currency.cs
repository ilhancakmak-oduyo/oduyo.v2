namespace Oduyo.Domain.Entities
{
    public class Currency : EntityBase
    {
        public string Code { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
