using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductNotification.Domain.Interfaces.Mensageria;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProductNotification.Infrastructure.RabbitMQ
{
    public class MensageriaRabbitMQ : IMensageriaRabbitMQ
    {
        private readonly IConfiguration _configuration;
        private readonly ConnectionFactory _connectionFactory;
        private static Dictionary<string, IConnection> ConnectionRabbitMQ = new Dictionary<string, IConnection>();
        private readonly ILogger _logger;
        private Object thisLock = new Object();
        public MensageriaRabbitMQ(IConfiguration configuration, ILogger logger)
        {
            this._configuration = configuration;
            this._logger = logger;
            _connectionFactory = GetConnectionFactory();
        }
        private ConnectionFactory GetConnectionFactory()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _configuration["Mensageria:RabbitMQ:HostName"],
                UserName = _configuration["Mensageria:RabbitMQ:UserName"],
                Password = _configuration["Mensageria:RabbitMQ:Password"],
                Port = int.Parse(_configuration["Mensageria:RabbitMQ:Port"]),
                Uri = new Uri(_configuration["Mensageria:RabbitMQ:Uri"])
            };

            return connectionFactory;
        }

        private IConnection CreateConnection(ConnectionFactory connectionFactory)
        {
            try
            {
                lock (thisLock)
                {
                    if (!ConnectionRabbitMQ.ContainsKey("connection"))
                        ConnectionRabbitMQ["connection"] = connectionFactory.CreateConnection();
                }

                return ConnectionRabbitMQ["connection"];
            }
            catch (TimeoutException ex)
            {
                Console.WriteLine($"Erro ao tentar criar conexão: {ex.Message} - {ex.StackTrace}");
                return null;
            }
        }

        private QueueDeclareOk CreateQueue(string queueName, IConnection connection)
        {
            QueueDeclareOk queue;
            using (var channel = connection.CreateModel())
            {
                queue = channel.QueueDeclare(queueName, false, false, false, null);
            }
            return queue;
        }

        private bool WriteMessageOnQueue(string message, string queueName, IConnection connection)
        {
            using (var channel = connection.CreateModel())
            {
                channel.BasicPublish(string.Empty, queueName, null, Encoding.ASCII.GetBytes(message));
            }

            return true;
        }

        public async Task SendMessageAsync<TEntity>(TEntity entity)
        {
            IConnection connection = CreateConnection(_connectionFactory);
            QueueDeclareOk queue = CreateQueue(_configuration["Mensageria:RabbitMQ:QueueName"], connection);
            WriteMessageOnQueue(JsonConvert.SerializeObject(entity), _configuration["Mensageria:RabbitMQ:QueueName"], connection);

            await Task.CompletedTask;
        }

        public async Task ReceiveMessagesAsync<TEntity>(Action<TEntity> action)
        {
            IConnection connection = CreateConnection(_connectionFactory);

            using (var channel = connection.CreateModel())
            {

                channel.QueueDeclare(queue: _configuration["Mensageria:RabbitMQ:QueueName"],
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    
                    action(JsonConvert.DeserializeObject<TEntity>(Encoding.UTF8.GetString(body)));
                };

                channel.BasicConsume(_configuration["Mensageria:RabbitMQ:QueueName"], true, consumer);

                Console.ReadLine();

                await Task.CompletedTask;
            }
        }
    }
}