namespace Oduyo.Domain.Entities
{
    public class UserRole : EntityBase
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}