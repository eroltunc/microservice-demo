using MediatR;
using OrderService.Domain.Models;

namespace OrderService.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<bool>
    {
        //Address
        public string City { get; private set; }
        public string FullAddress { get; private set; }
        public string State { get; private set; }
        //BasketItems
        private readonly List<OrderItemDTO> _orderItems;
        public IEnumerable<OrderItemDTO> OrderItems => _orderItems;
        //Customer
        public string CustomerId { get; set; }
        public string CustomerName { get; private set; }
        public string CustomerEmail { get; set; }
        //Payment
        public string CardNumber { get; private set; }
        public string CardHolderName { get; private set; }
        public DateTime CardExpirationDate { get; private set; }
        public string CardCvcNumber { get; private set; }
        public int CardTypeId { get; private set; }
        
        public CreateOrderCommand()
        {
            _orderItems = new List<OrderItemDTO>();
        }
        public CreateOrderCommand
            (
            List<BasketItem> basketItems, 
            string customerId,
            string customerName, 
            string customerEmail, 
            string city, 
            string fullAddress, 
            string state, 
            string cardNumber, 
            string cardHolderName, 
            DateTime cardExpirationDate, 
            string cardCvcCode,
            int cardTypeId
            ) : this()
        {
            _orderItems = basketItems.Select(item => new OrderItemDTO()
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                ProductPictureUrl = item.ProductPictureUrl,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity
            }).ToList();
            CustomerId= customerId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            City = city;
            FullAddress = fullAddress;
            State = state;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpirationDate = cardExpirationDate;
            CardCvcNumber = cardCvcCode;
            CardTypeId = cardTypeId;
            CardExpirationDate = cardExpirationDate;
        }
    }

    
}
