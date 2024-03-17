
using OrderService.Domain.AggregateModels.CustomerAggregate;
using OrderService.Domain.Events;
using OrderService.Domain.SeedWork;

namespace OrderService.Domain.AggregateModels.OrderAggregate
{
    public class Order : BaseEntity, IAggregateRoot
    {
        //Customer
        public Guid? CustomerId { get; private set; }
        public Customer Customer { get; private set; }
        //Address
        public Address Address { get; private set; }
        public string? Description { get; private set; }
        public int OrderStatusId { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;
        public Guid? PaymentMethodId { get; set; }
        protected Order()
        {
            Id = Guid.NewGuid();
            _orderItems = new List<OrderItem>();
        }
        public Order
            (
            Guid customerId,
            Address address,
            string customerName,
            int cardTypeId,
            string cardNumber,
            string cardCvcCode,
            string cardHolderName,
            DateTime cardExpiration,
            Guid? paymentMethodId,
            string? description = null
            ) : this()
        {
            CustomerId = customerId;
            Address = address;
            OrderStatusId = OrderStatus.Submitted.Id;
            PaymentMethodId = paymentMethodId;
            Description = description;
            AddOrderStartedDomainEvent(customerName, cardTypeId, cardNumber, cardCvcCode, cardHolderName, cardExpiration);
        }


        private void AddOrderStartedDomainEvent(string userName, int cardTypeId, string cardNumber, string cardCvcCode, string cardHolderName, DateTime cardExpirationDate) =>
            this.AddDomainEvent(new OrderStartedDomainEvent(this, userName, cardTypeId, cardNumber, cardCvcCode, cardHolderName, cardExpirationDate));
        public void AddOrderItem(Guid productId, string productName, decimal unitPrice, string productPictureUrl, int quantity) =>
            _orderItems.Add(new OrderItem(productId, productName, unitPrice, productPictureUrl, quantity));
        public void SetCustomerId(Guid customerId) =>
            CustomerId = customerId;
        public void SetPaymentMethodId(Guid paymentMethodId) =>
            PaymentMethodId = paymentMethodId;

    }
}
