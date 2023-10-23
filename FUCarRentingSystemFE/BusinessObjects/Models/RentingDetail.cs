using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusinessObjects.Models
{
    public partial class RentingDetail
    {
        public int RentingTransactionId { get; set; }
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        [JsonIgnore]
        public string FormattedStartDate => StartDate.ToString("MM-dd-yyyy");
        public DateTime EndDate { get; set; }
        [JsonIgnore]
        public string FormattedEndDate => EndDate.ToString("MM-dd-yyyy");

        public decimal? Price { get; set; }

        public virtual CarInformation Car { get; set; } = null!;
        public virtual RentingTransaction RentingTransaction { get; set; } = null!;
    }
}
