using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Models
{
    public partial class CarInformation
    {
        public CarInformation()
        {
            RentingDetails = new HashSet<RentingDetail>();
        }

        public int CarId { get; set; }
        public string CarName { get; set; } = null!;
        public string? CarDescription { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Car Number Of Doors must be greater than 0.")]
        public int? NumberOfDoors { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Car's Seating Capacity must be greater than 0.")]
        public int? SeatingCapacity { get; set; }
        public string? FuelType { get; set; }
        [Range(1900, 2023)]
        public int? Year { get; set; }
        public int ManufacturerId { get; set; }
        public int SupplierId { get; set; }
        public byte? CarStatus { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Car renting price per day must be greater than 0.")]
        public decimal CarRentingPricePerDay { get; set; }
        public virtual Manufacturer Manufacturer { get; set; } = null!;
        public virtual Supplier Supplier { get; set; } = null!;
        public virtual ICollection<RentingDetail> RentingDetails { get; set; }
    }
}
