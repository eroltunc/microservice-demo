using EventBus.Base.Abstraction;
using EventBus.Base.Events;
using PaymentService.Api.IntegrationEvents.Events;

namespace PaymentService.Api.IntegrationEvents.EventHandlers
{
    public class OrderStartedIntegrationEventHandler( IEventBus eventBus) : IIntegrationEventHandler<OrderStartedIntegrationEvent>
    {
        private readonly IEventBus _eventBus = eventBus;
        public Task Handle(OrderStartedIntegrationEvent @event)
        {
            IntegrationEvent paymentEvent = new OrderPaymentSuccessIntegrationEvent(@event.OrderId);
            //IntegrationEvent paymentEvent = new OrderPaymentFailedIntegrationEvent(@event.OrderId, "This is a fake error message");     
            _eventBus.Publish(paymentEvent);
            return Task.CompletedTask;
        }
    }
}
