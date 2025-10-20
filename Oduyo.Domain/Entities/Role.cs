using Microsoft.AspNetCore.Identity;
using Oduyo.Domain.Enums;

namespace Oduyo.Domain.Entities
{
    public class Role : IdentityRole<int>
    {
        public string Description { get; set; }
        public RoleType RoleType { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}