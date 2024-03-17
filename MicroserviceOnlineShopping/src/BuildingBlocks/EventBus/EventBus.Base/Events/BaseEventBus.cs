﻿using EventBus.Base.Abstraction;
using EventBus.Base.SubManagers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace EventBus.Base.Events
{
    public abstract class BaseEventBus : IEventBus
    {
        public readonly IServiceProvider ServiceProvider;
        public readonly IEventBusSubscriptionManager SubsManager;
        public EventBusConfig EventBusConfig { get; set; }

        public BaseEventBus(EventBusConfig config, IServiceProvider serviceProvider)
        {
            EventBusConfig = config;
            ServiceProvider = serviceProvider;
            SubsManager = new InMemoryEventBusSubscriptionManager(ProcessEventName);
        }
        public virtual string ProcessEventName(string eventName)
        {
            if (EventBusConfig.DeleteEventPrefix)
            {
                eventName = eventName.TrimStart(EventBusConfig.EventNamePrefix.ToArray());
            }
            if (EventBusConfig.DeleteEventSuffix && eventName.Contains(EventBusConfig.EventNameSuffix)) 
            {
                eventName = eventName.Substring(0, eventName.Length - EventBusConfig.EventNameSuffix.Length);      
            }
            return eventName;
        }
        public virtual string GetSubName  (string eventName) =>
        
           $"{EventBusConfig.SubscriberClientAppName}.{ProcessEventName(eventName)}";
        
        public virtual void Dispose()
        {
            EventBusConfig = null;
            SubsManager.Clear();
        }
        public async Task<bool> ProcessEvent(string eventName,string message)
        {
            eventName=ProcessEventName(eventName);
            var processed = false;
            if (SubsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = SubsManager.GetHandlersForEvent(eventName);
                using ( var scope = ServiceProvider.CreateScope())
                {
                    foreach (var subscription in subscriptions)
                    {
                        var handler = ServiceProvider.GetService(subscription.HandlerType);
                        if (handler == null) continue;
                        string eventFullName = $"{EventBusConfig.EventNamePrefix}{eventName}{EventBusConfig.EventNameSuffix}";//OrderStartedIntegrationEvent
                        var eventType=SubsManager.GetEventTypeByName(eventFullName);
                        var integrationEvent =JsonConvert.DeserializeObject(message, eventType);
                        var concreteType =typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
                processed = true;
            }
            return processed;
        }
        public abstract void Publish(IntegrationEvent @event);

        public abstract void Subscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;

        public abstract void UnSubscribe<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>;
    }
}
