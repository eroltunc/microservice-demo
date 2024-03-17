
using System.ComponentModel.DataAnnotations;

namespace BasketService.Entity.Models
{
    public class BasketItem : IValidatableObject
    {
        public string Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ProductPictureUrl { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (Quantity < 1)
            {
                results.Add(new ValidationResult("Invalid number", new[] { "Quantity" }));
            }
            return results;
        }
    }
}
