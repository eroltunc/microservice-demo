using EventBus.Base.Abstraction;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderService.Api.IntegrationEvents.Events;
using OrderService.Application.Features.Commands.CreateOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Api.IntegrationEvents.EventHandlers
{
    public class OrderCreatedIntegrationEventHandler(IServiceProvider serviceProvider,ILogger<OrderCreatedIntegrationEventHandler> logger) : IIntegrationEventHandler<OrderCreatedIntegrationEvent>
    {


        private readonly IServiceProvider _serviceProvider = serviceProvider; 
        private readonly ILogger<OrderCreatedIntegrationEventHandler> _logger=logger;

        public async Task Handle(OrderCreatedIntegrationEvent @event)
        {
          

            var createOrderCommand = new CreateOrderCommand
                (
                @event.Basket.Items,
                @event.CustomerId, 
                @event.CustomerName,
                @event.CustomerEmail,
                @event.City, 
                @event.FullAddress,
                @event.State,
                @event.CardNumber,
                @event.CardHolderName,
                @event.CardExpirationDate,
                @event.CardCvcCode, 
                @event.CardTypeId);
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(createOrderCommand);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
            }
        }
    }
}
