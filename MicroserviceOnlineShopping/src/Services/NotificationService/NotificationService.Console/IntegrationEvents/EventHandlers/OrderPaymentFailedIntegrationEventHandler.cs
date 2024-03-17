using EventBus.Base.Abstraction;
using NotificationService.Console.IntegrationEvents.Events;

namespace NotificationService.Console.IntegrationEvents.EventHandlers
{
    class OrderPaymentFailedIntegrationEventHandler : IIntegrationEventHandler<OrderPaymentFailedIntegrationEvent>
    {
        public async Task Handle(OrderPaymentFailedIntegrationEvent @event)
        {

            try
            {
                System.Console.WriteLine($"New Notification {@event.OrderId} \n    Order Payment failed \n    Event Name: {@event} \n");
            }
            catch (Exception ex)
            {

                System.Console.WriteLine(ex.ToString());
            }
           
 
        }
    }
}
