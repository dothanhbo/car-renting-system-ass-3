using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObjects.Models
{
    public partial class RentingTransaction
    {
        public RentingTransaction()
        {
            RentingDetails = new HashSet<RentingDetail>();
        }

        public int RentingTransationId { get; set; }
        public DateTime? RentingDate { get; set; }
        [JsonIgnore]
        public string FormattedRentingDate => RentingDate.Value.ToString("MM-dd-yyyy");
        public decimal? TotalPrice { get; set; }
        public int CustomerId { get; set; }
        public byte? RentingStatus { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<RentingDetail> RentingDetails { get; set; }
    }
}
