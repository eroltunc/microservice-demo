using MediatR;
using OrderService.Domain.AggregateModels.CustomerAggregate;

namespace OrderService.Domain.Events
{
    public class CustomerAndPaymentMethodVerifiedDomainEvent : INotification
    {
        public Customer Buyer { get; private set; }
        public PaymentMethod Payment { get; private set; }
        public Guid OrderId { get; private set; }

        public CustomerAndPaymentMethodVerifiedDomainEvent(Customer buyer, PaymentMethod payment, Guid orderId)
        {
            Buyer = buyer;
            Payment = payment;
            OrderId = orderId;
        }
    }
}
