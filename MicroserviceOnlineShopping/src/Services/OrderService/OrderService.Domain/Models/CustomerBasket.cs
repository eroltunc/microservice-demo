namespace OrderService.Domain.Models
{
    public class CustomerBasket
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public CustomerBasket(string customerId, string customerName, string customerEmail)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;      
        }
    }
}
