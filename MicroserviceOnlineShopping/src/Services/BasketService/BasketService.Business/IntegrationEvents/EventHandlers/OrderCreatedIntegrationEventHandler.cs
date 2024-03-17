using BasketService.Application.IntegrationEvents.Events;
using BasketService.DataAccess.Abstract;
using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;


namespace BasketService.Application.IntegrationEvents.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler(IBasketDal repository) : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {
        private readonly IBasketDal _repository = repository;  
        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
           await _repository.DeleteBasketAsync(@event.CustomerId);
        }
    }
}
