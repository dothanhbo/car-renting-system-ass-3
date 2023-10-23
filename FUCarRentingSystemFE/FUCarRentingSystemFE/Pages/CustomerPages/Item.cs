using BusinessObjects.Models;
using System.ComponentModel.DataAnnotations;

namespace SimpleShoppingCartSession.Models
{
    public class Item
    {
        public CarInformation CarInformation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public decimal Total { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult("End Date must be greater than Start Date.", new[] { nameof(EndDate) });
            }
        }
    }
}
