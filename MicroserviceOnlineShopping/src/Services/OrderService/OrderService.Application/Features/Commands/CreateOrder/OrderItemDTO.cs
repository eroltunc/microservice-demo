namespace OrderService.Application.Features.Commands.CreateOrder
{
    public class OrderItemDTO
    {
        public Guid ProductId { get; init; }
        public string ProductName { get; init; }
        public decimal UnitPrice { get; init; }
        public int Quantity { get; init; }
        public string ProductPictureUrl { get; init; }
    }
}
