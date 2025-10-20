namespace Oduyo.Domain.Entities
{
    public class UserHierarchy : EntityBase
    {
        public int ParentUserId { get; set; }
        public int ChildUserId { get; set; }
    }
}