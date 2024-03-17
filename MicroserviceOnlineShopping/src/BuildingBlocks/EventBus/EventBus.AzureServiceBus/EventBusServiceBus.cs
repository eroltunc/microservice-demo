using EventBus.Base;
using EventBus.Base.Events;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using System.Text;

namespace EventBus.AzureServiceBus
{
    public class EventBusServiceBus: BaseEventBus
    {
        private ITopicClient _topicClient;
        private ManagementClient _managementClient;
        public EventBusServiceBus(EventBusConfig eventBusConfig, IServiceProvider serviceProvider) : base(eventBusConfig, serviceProvider)
        {
            _managementClient = new ManagementClient(eventBusConfig.EventBusConnectionString);
            _topicClient = createTopicClient();
        }
        private ITopicClient createTopicClient()
        {
            if (_topicClient == null || _topicClient.IsClosedOrClosing) 
            {
                _topicClient = new TopicClient(EventBusConfig.EventBusConnectionString, EventBusConfig.DefaultTopicName, RetryPolicy.Default);
            }
            if (!_managementClient.TopicExistsAsync(EventBusConfig.DefaultTopicName).GetAwaiter().GetResult())
            {
                _managementClient.CreateTopicAsync(EventBusConfig.DefaultTopicName).GetAwaiter().GetResult();
            } 
            return _topicClient;
        }

        public override void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name; 
            eventName=ProcessEventName(eventName); 
            var message = new Message()
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = null,
                Label=eventName
            
            };
            _topicClient.SendAsync(message).GetAwaiter().GetResult();
        }

        public override void Subscribe<T, TH>()
        {var eventName=typeof(T).Name;

           
            eventName=ProcessEventName(eventName);
            if (!SubsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptionClient = CreateSubscriptionClientIfNotExists(eventName);
                RegisterSubscriptionClientMessageHandler(subscriptionClient);
            }
            SubsManager.AddSubscription<T,TH > ();
        }

        public override void UnSubscribe<T, TH>()
        {
           var eventName=typeof(T).Name;
            try
            {
                var subscriptionClient = createSubscriptionClient(eventName);
                subscriptionClient.RemoveRuleAsync(eventName).GetAwaiter().GetResult();
            }
            catch (MessagingEntityNotFoundException)
            {
            
            }
            SubsManager.RemoveSubscription<T,TH> ();
        }
        private void RegisterSubscriptionClientMessageHandler(ISubscriptionClient subscriptionClient) 
        {
            subscriptionClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var eventName = $"{message.Label}";
                    var messageData = Encoding.UTF8.GetString(message.Body);
                    if (await ProcessEvent(ProcessEventName(eventName), messageData))
                    {
                        await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                    }
                },
                new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
          
        }
        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var ex=exceptionReceivedEventArgs.Exception;
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            return Task.CompletedTask;
        }
        private ISubscriptionClient CreateSubscriptionClientIfNotExists(String eventName)
        {
            var subClient=createSubscriptionClient(eventName);
            var exist=_managementClient.SubscriptionExistsAsync(EventBusConfig.DefaultTopicName,GetSubName(eventName)).GetAwaiter().GetResult();
            if (!exist)
            {
                _managementClient.CreateSubscriptionAsync(EventBusConfig.DefaultTopicName, GetSubName(eventName)).GetAwaiter().GetResult();
                RemoveDefaultRule(subClient);
            }
            CreateRuleIfNtExist(ProcessEventName(eventName),subClient);
            return subClient;
        }
        private void CreateRuleIfNtExist(string eventName,ISubscriptionClient subscriptionClient)
        {
            bool ruleExist;
            try
            {
                var rule = _managementClient.GetRuleAsync(EventBusConfig.DefaultTopicName, GetSubName(eventName), eventName).GetAwaiter().GetResult();
                ruleExist = rule != null;
            }
            catch (Exception)
            {
                ruleExist=false;
               
            }
            if (!ruleExist)
            {
                subscriptionClient.AddRuleAsync(new RuleDescription
                {
                    Filter=new CorrelationFilter { Label=eventName},
                    Name=eventName
                }).GetAwaiter().GetResult();
            }
        }
        private void RemoveDefaultRule(SubscriptionClient subscriptionClient)
        {
            try
            {
                subscriptionClient.RemoveRuleAsync(RuleDescription.DefaultRuleName).GetAwaiter().GetResult();
            }
            catch (MessagingEntityNotFoundException)
            {
                
            }
        }
        private SubscriptionClient createSubscriptionClient(string eventName)
        {
            return new SubscriptionClient(EventBusConfig.EventBusConnectionString, EventBusConfig.DefaultTopicName, GetSubName(eventName));
        }
        public override void Dispose()
        {
            base.Dispose();
            _topicClient.CloseAsync().GetAwaiter().GetResult();
            _managementClient.CloseAsync().GetAwaiter().GetResult();
            _topicClient = null;
            _managementClient = null;

        }
    }
}
