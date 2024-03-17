using EventBus.Base.Abstraction;
using EventBus.Base.Events;

namespace EventBus.Base.SubManagers
{
    public class InMemoryEventBusSubscriptionManager : IEventBusSubscriptionManager
    {
        private readonly Dictionary<string, List<SubscriptionInfo>> _handlers;
        private readonly List<Type> _eventTypes;
        public event EventHandler<string> _onEventRemoved;
        public Func<string, string> _eventNameGetter;
        public InMemoryEventBusSubscriptionManager(Func<string, string> eventNameGetter)
        {
            _handlers=new Dictionary<string, List<SubscriptionInfo>>();
            _eventTypes=new List<Type>();
            _eventNameGetter = eventNameGetter;
        }

        public bool IsEmpty => !_handlers.Any();
        public void Clear()=> _handlers.Clear();
        public void AddSubscription<T, TH>() where T : IntegrationEvent where TH : IIntegrationEventHandler<T>
        {
            var eventName = GetEventKey<T>();
            AddSubscription(typeof(TH), eventName);

            if (!_eventTypes.Contains(typeof(T)))
            {
                _eventTypes.Add(typeof(T));
            }
        }
        private void AddSubscription(Type handlerType,string eventName)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<SubscriptionInfo>());
            }
            if (_handlers[eventName].Any(s=>s.HandlerType==handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} already registrad for '{eventName}'", nameof(handlerType));
            }
            _handlers[eventName].Add(SubscriptionInfo.Typed(handlerType));
        }
        public void RemoveSubscription<T, TH>() where T : IntegrationEvent  where TH : IIntegrationEventHandler<T> =>
            RemoveHandler(GetEventKey<T>(), FindSubsrictionToRemove<T, TH>());
        
        private void RemoveHandler(string eventName, SubscriptionInfo subsToRemove)
        {
            if(subsToRemove != null)
            {
                _handlers[eventName].Remove(subsToRemove);
                if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    var eventType =_eventTypes.SingleOrDefault(e=>e.Name==eventName);
                    if (eventType != null)
                    {
                        _eventTypes.Remove(eventType);
                    }
                    RaiseOnEventRemoved(eventName);
                }
            }
        }
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent<T>() where T : IntegrationEvent=>    
            GetHandlersForEvent(GetEventKey<T>());
       
        public IEnumerable<SubscriptionInfo> GetHandlersForEvent(string eventName) => _handlers[eventName];
        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = _onEventRemoved;
            handler?.Invoke(this, eventName);
        }
        private SubscriptionInfo FindSubsrictionToRemove<T,TH>() where T:IntegrationEvent where TH : IIntegrationEventHandler<T> =>
            FindSubstictionToRemove(GetEventKey<T>(), typeof(TH));
      
        private SubscriptionInfo FindSubstictionToRemove(string eventName, Type handlerType)=>
            (!HasSubscriptionsForEvent(eventName))?null : _handlers[eventName].SingleOrDefault(z => z.HandlerType == handlerType);

       
        public bool HasSubscriptionsForEvent<T>() where T : IntegrationEvent =>
            HasSubscriptionsForEvent(GetEventKey<T>());
       
        public bool HasSubscriptionsForEvent(string eventName)=> 
            _handlers.ContainsKey(eventName);
        public Type GetEventTypeByName(string eventName) => 
            _eventTypes.SingleOrDefault(z => z.Name == eventName);
        public string GetEventKey<T>()=>
            _eventNameGetter(typeof(T).Name);
       
    }
}
