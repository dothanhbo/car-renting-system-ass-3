using System;
using System.Collections.Generic;

namespace FUCarRentingSystem.DTO
{
    public partial class CarInformationDto
    {
        public int CarId { get; set; }
        public string CarName { get; set; } = null!;
        public string? CarDescription { get; set; }
        public int? NumberOfDoors { get; set; }
        public int? SeatingCapacity { get; set; }
        public string? FuelType { get; set; }
        public int? Year { get; set; }
        public int ManufacturerId { get; set; }
        public int SupplierId { get; set; }
        public byte? CarStatus { get; set; }
        public decimal? CarRentingPricePerDay { get; set; }
    }
}
