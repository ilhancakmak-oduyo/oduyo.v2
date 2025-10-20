namespace Oduyo.Domain.Entities
{
    public class PackageModule : EntityBase
    {
        public int PackageId { get; set; }
        public int ModuleId { get; set; }
        public bool IsFree { get; set; }
    }
}