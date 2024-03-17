using EventBus.Base.Abstraction;
using Microsoft.Extensions.Logging;
using NotificationService.Console.IntegrationEvents.Events;

namespace NotificationService.Console.IntegrationEvents.EventHandlers
{
    public class UserLoggedIntegrationEventHandler : IIntegrationEventHandler<UserLoggedIntegrationEvent>
    {
      
        public async Task Handle(UserLoggedIntegrationEvent @event)
        {
            try
            {
                System.Console.WriteLine($"New Notification {@event.Id} \n    Full Name: {@event.FullName}\n    Event Name: {@event} \n");
            }
            catch (Exception ex)
            {

                System.Console.WriteLine(ex.ToString());
            }
        }
    }
}