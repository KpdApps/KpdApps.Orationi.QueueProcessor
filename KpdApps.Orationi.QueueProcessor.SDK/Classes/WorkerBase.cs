using System.Collections.Generic;
using KpdApps.Orationi.QueueProcessor.SDK.Interfaces;
using System;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace KpdApps.Orationi.QueueProcessor.SDK
{
    public abstract class WorkerBase : IWorker, IDisposable
    {
        public bool IsFinished { get; set; }

        string _queueName = string.Empty;
        public string QueueName
        {
            get => _queueName;
            set
            {
                if (_queueName == value)
                {
                    return;
                }

                _queueName = value;
            }
        }

        bool _durable = false;
        public bool Durable
        {
            get => _durable;
            set
            {
                if (_durable == value)
                {
                    return;
                }
                _durable = value;
            }
        }

        bool _exclusive = false;
        public bool Exclusive
        {
            get => _exclusive;
            set
            {
                if (_exclusive == value)
                {
                    return;
                }
                _exclusive = value;
            }
        }

        bool _autoDetect = false;
        public bool AutoDelete
        {
            get => _autoDetect;
            set
            {
                if (_autoDetect == value)
                {
                    return;
                }
                _autoDetect = value;
            }
        }

        IDictionary<string, object> _arguments = null;
        public IDictionary<string, object> Arguments
        {
            get => _arguments;
            set
            {
                if (Equals(_arguments, value))
                {
                    return;
                }

                _arguments = value;
            }
        }

        bool _autoAck = true;
        public bool AutoAck
        {
            get => _autoAck;
            set
            {
                if (_autoAck == value)
                {
                    return;
                }
                _autoAck = value;
            }
        }

        string _hostname = string.Empty;
        public string Hostname
        {
            get => _hostname;
            set
            {
                if (_hostname == value)
                {
                    return;
                }
                _hostname = value;
            }
        }

        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _consumer;

        public WorkerBase()
        {
        }

        ~WorkerBase()
        {
            Dispose();
        }

        public virtual void Connect(string hostname)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(_queueName, _durable, _exclusive, _autoDetect, _arguments);
            _channel.BasicQos(0, 1, false);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += Consumer_Received;
        }


        public virtual void BasicConsume()
        {
            _channel.BasicConsume(_queueName, _autoAck, _consumer);
        }

        protected abstract void Consumer_Received(object sender, BasicDeliverEventArgs e);

        public void Dispose()
        {
            if (_channel != null)
            {
                _channel.Dispose();
            }

            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}
