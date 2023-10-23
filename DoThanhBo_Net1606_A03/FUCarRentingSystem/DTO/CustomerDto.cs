namespace FUCarRentingSystem.DTO
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = default!;
        public string Mobile { get; set; } = default!;
        public DateTime Birthday { get; set; }
        public string IdentityCard { get; set; } = default!;
        public string LicenceNumber { get; set; } = default!;
        public DateTime LicenceDate { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
