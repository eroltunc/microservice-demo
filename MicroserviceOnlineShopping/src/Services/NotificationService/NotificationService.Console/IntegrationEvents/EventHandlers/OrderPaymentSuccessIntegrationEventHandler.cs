using EventBus.Base.Abstraction;
using NotificationService.Console.IntegrationEvents.Events;

namespace NotificationService.Console.IntegrationEvents.EventHandlers
{
    class OrderPaymentSuccessIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentSuccessIntegrationEvent>
    {

        public async Task Handle(OrderPaymentSuccessIntegrationEvent @event)
        {
           
            try
            {
                System.Console.WriteLine($"New Notification {@event.OrderId} \n    Order Payment Success \n    Event Name: {@event} \n");
            }
            catch (Exception ex)
            {

                System.Console.WriteLine(ex.ToString());
            }
        }
    }
}
