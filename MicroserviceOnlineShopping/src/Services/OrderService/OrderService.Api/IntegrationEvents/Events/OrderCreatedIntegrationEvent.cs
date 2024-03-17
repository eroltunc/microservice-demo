using EventBus.Base.Events;
using OrderService.Domain.Models;

namespace OrderService.Api.IntegrationEvents.Events
{
    public class OrderCreatedIntegrationEvent : IntegrationEvent
    {
        public string CustomerId { get; }
        public string CustomerName { get; }
        public string CustomerEmail { get; }
        public int OrderNumber { get; set; }
        public string City { get; set; }
        public string FullAddress { get; set; }
        public string State { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public DateTime CardExpirationDate { get; set; }
        public string CardCvcCode { get; set; }
        public int CardTypeId { get; set; }      
        public Guid RequestId { get; set; }
        public CustomerBasket Basket { get; }
        public OrderCreatedIntegrationEvent
            (
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
            int cardTypeId,
            CustomerBasket basket
            )
        {
            CustomerId = customerId;
            CustomerName = customerName;
            CustomerEmail = customerEmail;
            City = city;
            FullAddress = fullAddress;
            State = state;
            CardNumber = cardNumber;
            CardHolderName = cardHolderName;
            CardExpirationDate = cardExpirationDate;
            CardCvcCode = cardCvcCode;
            CardTypeId = cardTypeId;
            Basket = basket;
        }

    }
}
