﻿using EventBus.Base;
using EventBus.Base.Events;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public class EventBusRabbitMQ : BaseEventBus
    {
        RabbitMQPersistentConnection _rabbitMQPersistentConnection;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IModel _consumerChannel;
        public EventBusRabbitMQ(EventBusConfig eventBusConfig, IServiceProvider serviceProvider) : base(eventBusConfig, serviceProvider)
        {
            if (EventBusConfig.Connection != null)
            {
                if (EventBusConfig.Connection is ConnectionFactory)
                    _connectionFactory = EventBusConfig.Connection as ConnectionFactory;
                else
                {
                    var connJson = JsonConvert.SerializeObject(EventBusConfig.Connection, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    _connectionFactory = JsonConvert.DeserializeObject<ConnectionFactory>(connJson);
                }
            }
            else
                _connectionFactory = new ConnectionFactory();

            _rabbitMQPersistentConnection = new RabbitMQPersistentConnection(_connectionFactory, eventBusConfig.ConnectionRetryCount);

            _consumerChannel = CreateConsumerChannel();

            SubsManager._onEventRemoved += SubsManager_OnEventRemoved;           
        }
        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            eventName = ProcessEventName(eventName);

            if (!_rabbitMQPersistentConnection.IsConnected)          
                _rabbitMQPersistentConnection.TryConnect();
           
            _consumerChannel.QueueUnbind(queue: eventName, exchange: EventBusConfig.DefaultTopicName,routingKey: eventName);

            if (SubsManager.IsEmpty)
                _consumerChannel.Close();
        }
        public override void Publish(IntegrationEvent @event)
        {
            if (!_rabbitMQPersistentConnection.IsConnected)
                _rabbitMQPersistentConnection.TryConnect();
           
            var policy = Policy
                .Handle<BrokerUnreachableException>().Or<SocketException>()
                .WaitAndRetry(EventBusConfig.ConnectionRetryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { });
            var eventName = @event.GetType().Name;
            eventName = ProcessEventName(eventName);

            _consumerChannel.ExchangeDeclare(exchange: EventBusConfig.DefaultTopicName, type: "direct");

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);
            policy.Execute(() =>
            {
                var properties = _consumerChannel.CreateBasicProperties();
                properties.DeliveryMode = 2;              
                _consumerChannel.BasicPublish(exchange: EventBusConfig.DefaultTopicName,
                    routingKey: eventName, mandatory: true,
                    basicProperties: properties, body: body);
            });
        }

        public override void Subscribe<T, TH>()
        {
            var eventName = typeof(T).Name;
            eventName = ProcessEventName(eventName);
            if (!SubsManager.HasSubscriptionsForEvent(eventName))
            {
                if (!_rabbitMQPersistentConnection.IsConnected)
                    _rabbitMQPersistentConnection.TryConnect();
               
                _consumerChannel.QueueDeclare(queue: GetSubName(eventName),
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                _consumerChannel.QueueBind(queue: GetSubName(eventName),
                    exchange: EventBusConfig.DefaultTopicName,
                    routingKey: eventName);
            }
            SubsManager.AddSubscription<T, TH>();
            StartBasicConsumer(eventName);
        }

        public override void UnSubscribe<T, TH>() =>
            SubsManager.RemoveSubscription<T, TH>();
        
        private IModel CreateConsumerChannel()
        {
            if (!_rabbitMQPersistentConnection.IsConnected)
                _rabbitMQPersistentConnection.TryConnect();
            
            var channel = _rabbitMQPersistentConnection.CreateModel();
            channel.ExchangeDeclare(exchange: EventBusConfig.DefaultTopicName, type: "direct");
            return channel;
        }
        private void StartBasicConsumer(string eventName)
        {
            if (_consumerChannel != null)
            {
                var consumer = new EventingBasicConsumer(_consumerChannel);
                consumer.Received += Consumer_Received;
                _consumerChannel.BasicConsume(queue: GetSubName(eventName), autoAck: false, consumer: consumer);
            }
        }
        private async void Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            eventName = ProcessEventName(eventName);
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
           
            try
            {
                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

            }
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }
    }
}
