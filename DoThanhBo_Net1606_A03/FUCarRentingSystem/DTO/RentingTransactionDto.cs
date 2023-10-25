using DataAcessObjects.DAO;

namespace FUCarRentingSystem.DTO
{
    public class RentingTransactionDto
    {
        public int RentingTransationId { get; set; }
        public DateTime? RentingDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public byte? RentingStatus { get; set; }
        public virtual CustomerDto? Customer { get; set; } = null!;
        public virtual ICollection<RentingDetailDto>? RentingDetails { get; set; }
    }
}
