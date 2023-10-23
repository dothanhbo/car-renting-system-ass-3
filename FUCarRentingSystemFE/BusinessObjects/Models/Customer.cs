using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace BusinessObjects.Models
{
    public partial class Customer
    {
        public Customer()
        {
            RentingTransactions = new HashSet<RentingTransaction>();
        }

        public int CustomerId { get; set; }
        [MaxLength(50)]
        public string CustomerName { get; set; }
        [MaxLength(12)]
        public string? Telephone { get; set; }
        [Required()]
        [MaxLength(50)]
        public string Email { get; set; } = null!;
        [DateRange("01/01/1900", "12/31/2020", ErrorMessage = "The Birthday must be from 01/01/1900 to 12/31/2020.")]
        public DateTime? CustomerBirthday { get; set; }
        [JsonIgnore]
        public string? FormattedRentingDate => CustomerBirthday?.ToString("MM-dd-yyyy");
        public byte? CustomerStatus { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }

        public virtual ICollection<RentingTransaction> RentingTransactions { get; set; }
    }
}
