using Oduyo.Domain.Enums;

namespace Oduyo.Domain.DTOs
{
    public class CreateRoleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public RoleType RoleType { get; set; }
    }
}