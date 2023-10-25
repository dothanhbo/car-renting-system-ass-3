namespace FUCarRentingSystem.DTO
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = default!;
        public string Telephone { get; set; } = default!;
        public string Email { get; set; } = null!;
        public DateTime CustomerBirthday { get; set; }
        public byte CustomerStatus { get; set; }
        public string? Password { get; set; }
    }
}
