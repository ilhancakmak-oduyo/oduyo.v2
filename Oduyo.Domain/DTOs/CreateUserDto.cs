using Oduyo.Domain.Enums;

namespace Oduyo.Domain.DTOs
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
    }
}