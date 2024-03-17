using OrderService.Domain.SeedWork;
using System.ComponentModel.DataAnnotations;

namespace OrderService.Domain.AggregateModels.OrderAggregate
{
    public class OrderItem : BaseEntity, IValidatableObject
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductPictureUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        protected OrderItem() {}
        public OrderItem(Guid productId, string productName, decimal unitPrice, string productPictureUrl, int quantity)
        {
            ProductId = productId;
            ProductName = productName;
            UnitPrice = unitPrice;
            ProductPictureUrl = productPictureUrl;
            Quantity = quantity;            
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (Quantity <= 0)
                results.Add(new ValidationResult("Invalid number of units", new[] { "Units" }));
            return results;
        }
    }
}
