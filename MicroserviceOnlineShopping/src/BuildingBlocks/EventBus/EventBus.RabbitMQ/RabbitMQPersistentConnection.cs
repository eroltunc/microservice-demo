using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public class RabbitMQPersistentConnection : IDisposable
    {


        private readonly IConnectionFactory _connectionFactory;
        private readonly int retyCount;

        private IConnection _connection;
        private object _lock_object = new object();
        private bool _disposed;

        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retyCount = 5)
        {
            _connectionFactory = connectionFactory;
        }

        public bool IsConnected =>
            _connection != null && _connection.IsOpen;
        public IModel CreateModel()=>
            _connection.CreateModel();
       
        public void Dispose()
        {
            _disposed = true;
            _connection.Dispose();
        }
        public bool TryConnect()
        {
            lock (_lock_object)
            {
                var policy = Policy
                    .Handle<SocketException>().Or<BrokerUnreachableException>()
                    .WaitAndRetry(retyCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) => { } );
                policy.Execute(() =>
                {
                    _connection = _connectionFactory.CreateConnection();
                });
                if (IsConnected)
                {
                    _connection.ConnectionShutdown += Connection_ConnectionShutdown;
                    _connection.CallbackException += Connection_CallbackException;
                    _connection.ConnectionBlocked += Connection_ConnectionBlocked;                  
                    return true;
                }
                return false;
            }


        }

        private void Connection_CallbackException(object? sender, global::RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            TryConnect();
        }

        private void Connection_ConnectionBlocked(object? sender, global::RabbitMQ.Client.Events.ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            TryConnect();
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (_disposed) return;
            TryConnect();
        }
    }
}
