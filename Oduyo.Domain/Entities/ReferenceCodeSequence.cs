namespace Oduyo.Domain.Entities
{
    public class ReferenceCodeSequence : EntityBase
    {
        public string Prefix { get; set; }
        public int CurrentValue { get; set; }
    }
}
