namespace Oduyo.Domain.DTOs
{
    public class CreateTenantDto
    {
        public string Name { get; set; }
        public string Domain { get; set; }
        public string ConnectionString { get; set; }
    }
}