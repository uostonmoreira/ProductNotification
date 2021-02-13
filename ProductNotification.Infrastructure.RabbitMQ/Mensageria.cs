using ProductNotification.Domain.Interfaces.Mensageria;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ProductNotification.Infrastructure.RabbitMQ
{
    public class Mensageria<TEntity> : IMensageria<TEntity>
    {
        private readonly ConnectionFactory _connectionFactory;
        private static Dictionary<string, IConnection> ConnectionRabbitMQ = new Dictionary<string, IConnection>();
        private Object thisLock = new Object();
        public Mensageria()
        {
            _connectionFactory = GetConnectionFactory();
        }
        private ConnectionFactory GetConnectionFactory()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "grouse.rmq.cloudamqp.com",
                UserName = "sqimllhs",
                Password = "bwkz92MyLJz3j3azAITdrRKGAtIjsJnj",
                Port = 1883,
                Uri = new Uri("amqps://sqimllhs:bwkz92MyLJz3j3azAITdrRKGAtIjsJnj@grouse.rmq.cloudamqp.com/sqimllhs")
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

        public void EnviaMensagem(TEntity obj, string fila)
        {
            IConnection connection = CreateConnection(_connectionFactory);
            QueueDeclareOk queue = CreateQueue(fila, connection);
            WriteMessageOnQueue(JsonSerializer.Serialize(obj), fila, connection);
        }

        public void LerMensagem(string fila, Action<string> action)
        {
            IConnection connection = CreateConnection(_connectionFactory);

            using (var channel = connection.CreateModel())
            {

                channel.QueueDeclare(queue: fila,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    action(message);
                };

                channel.BasicConsume(fila, true, consumer);

                Console.ReadLine();
            }
        }
    }
}