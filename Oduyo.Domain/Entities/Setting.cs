namespace Oduyo.Domain.Entities
{
    public class Setting : EntityBase
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
