using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Console.IntegrationEvents.EventHandlers;
using NotificationService.Console.IntegrationEvents.Events;

var services = new ServiceCollection();
ConfigureServices(services);
var sp = services.BuildServiceProvider();

IEventBus eventBus = sp.GetRequiredService<IEventBus>();

eventBus.Subscribe<OrderPaymentSuccessIntegrationEvent, OrderPaymentSuccessIntegrationEventHandler>();
eventBus.Subscribe<OrderPaymentFailedIntegrationEvent, OrderPaymentFailedIntegrationEventHandler>();
eventBus.Subscribe<UserLoggedIntegrationEvent, UserLoggedIntegrationEventHandler>();
static void ConfigureServices(ServiceCollection services)
{
    services.AddTransient<OrderPaymentFailedIntegrationEventHandler>();
    services.AddTransient<OrderPaymentSuccessIntegrationEventHandler>();
    services.AddTransient<UserLoggedIntegrationEventHandler>();
    services.AddSingleton(sp =>
    {
        EventBusConfig config = new()
        {
            ConnectionRetryCount = 5,
            EventNameSuffix = "IntegrationEvent",
            SubscriberClientAppName = "NotificationService",
            EventBusType = EventBusType.RabbitMQ,           
        };

        return EventBusFactory.Create(config, sp);
    });
}
Console.WriteLine("Application is Running....");
Console.ReadLine();
