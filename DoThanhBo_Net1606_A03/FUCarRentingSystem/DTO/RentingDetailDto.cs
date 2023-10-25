using System;
using System.Collections.Generic;

namespace FUCarRentingSystem.DTO
{
    public partial class RentingDetailDto
    {
        public int RentingTransactionId { get; set; }
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal? Price { get; set; }
        public virtual CarInformationDto? Car { get; set; } = null!;
    }
}
