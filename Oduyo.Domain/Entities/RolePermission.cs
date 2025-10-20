namespace Oduyo.Domain.Entities
{
    public class RolePermission : EntityBase
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}