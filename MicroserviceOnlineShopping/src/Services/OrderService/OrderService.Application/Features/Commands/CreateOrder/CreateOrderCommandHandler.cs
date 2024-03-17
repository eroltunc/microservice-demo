using EventBus.Base.Abstraction;
using MediatR;
using OrderService.Application.IntegrationEvents;
using OrderService.Application.IntegrationEvents.Events;
using OrderService.Application.Interfaces.Repositories;
using OrderService.Domain.AggregateModels.OrderAggregate;

namespace OrderService.Application.Features.Commands.CreateOrder
{
    public class CreateOrderCommandHandler(IOrderRepository orderRepository, IEventBus eventBus) : IRequestHandler<CreateOrderCommand, bool>
    {

        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IEventBus _eventBus = eventBus;

     

        public async Task<bool> Handle(CreateOrderCommand createOrderCommand, CancellationToken cancellationToken)
        {
            Order dbOrder = new(
                Guid.Parse(createOrderCommand.CustomerId),
                new Address(createOrderCommand.City, createOrderCommand.State, createOrderCommand.FullAddress),
                createOrderCommand.CustomerName,                
                createOrderCommand.CardTypeId, 
                createOrderCommand.CardNumber, 
                createOrderCommand.CardCvcNumber,
                createOrderCommand.CardHolderName, 
                createOrderCommand.CardExpirationDate, 
                null);

            foreach (var item in createOrderCommand.OrderItems)
                dbOrder.AddOrderItem(item.ProductId, item.ProductName, item.UnitPrice, item.ProductPictureUrl, item.Quantity);
          

            await _orderRepository.AddAsync(dbOrder);
            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            var orderStartedIntegrationEvent = new OrderStartedIntegrationEvent(createOrderCommand.CustomerName, dbOrder.Id);
            _eventBus.Publish(orderStartedIntegrationEvent);
            
            return true;
        }
    }
}
