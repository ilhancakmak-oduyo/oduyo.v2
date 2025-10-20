namespace Oduyo.Domain.DTOs
{
    public class UpdateCompanyContactDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsPrimary { get; set; }
    }
}