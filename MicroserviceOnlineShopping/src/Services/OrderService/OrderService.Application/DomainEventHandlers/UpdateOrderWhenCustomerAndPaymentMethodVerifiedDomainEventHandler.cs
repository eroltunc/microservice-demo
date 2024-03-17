using MediatR;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.Events;

namespace OrderService.Application.DomainEventHandlers
{
    public class UpdateOrderWhenCustomerAndPaymentMethodVerifiedDomainEventHandler(IOrderRepository orderRepository) : INotificationHandler<CustomerAndPaymentMethodVerifiedDomainEvent>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        

        public async Task Handle(CustomerAndPaymentMethodVerifiedDomainEvent buyerPaymentMethodVerifiedEvent, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetByIdAsync(buyerPaymentMethodVerifiedEvent.OrderId);
            orderToUpdate.SetCustomerId(buyerPaymentMethodVerifiedEvent.Buyer.Id);
            orderToUpdate.SetPaymentMethodId(buyerPaymentMethodVerifiedEvent.Payment.Id);
        }
    }
}
