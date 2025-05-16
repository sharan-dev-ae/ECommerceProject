namespace CompanyService.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalAddress { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
    }

    public class CompanyCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalAddress { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
    }
}
