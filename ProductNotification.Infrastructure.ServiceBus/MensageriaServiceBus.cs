using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductNotification.Domain.Interfaces.Mensageria;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ProductNotification.Infrastructure.ServiceBus
{
    public class MensageriaServiceBus : IMensageriaServiceBus
    {
        private readonly string _connection;
        private readonly string _nomeFila;
        private readonly ILogger _logger;

        public MensageriaServiceBus(IConfiguration configuration, ILogger logger)
        {
            this._logger = logger;
            _nomeFila = configuration["Mensageria:ServiceBus:QueueName"];
            _connection = configuration["Mensageria:ServiceBus:Connection"];
        }

        public async Task SendMessageAsync<TEntity>(TEntity entity)
        {
            var sw = new Stopwatch();
            sw.Start();

            this._logger?.LogDebug($"Iniciando postagem na fila {this._nomeFila} em {sw.ElapsedMilliseconds}ms.");

            var queueClient = new QueueClient(this._connection, this._nomeFila);

            var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entity, Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                MissingMemberHandling = MissingMemberHandling.Ignore
                            })));

            await queueClient.SendAsync(message);

            sw.Stop();
            this._logger?.LogDebug($"Objeto postado na fila {this._nomeFila} em {sw.ElapsedMilliseconds}ms.");
        }

        public async Task ReceiveMessagesAsync<TEntity>(Action<TEntity> action)
        {
            var sw = new Stopwatch();
            sw.Start();

            this._logger?.LogDebug($"Iniciando consulta na fila {this._nomeFila} em {sw.ElapsedMilliseconds}ms.");

            var messageReceiver = new MessageReceiver(this._connection, this._nomeFila, ReceiveMode.PeekLock);
            Message message = await messageReceiver.ReceiveAsync();

            await messageReceiver.CompleteAsync(message.SystemProperties.LockToken);

            sw.Stop();
            this._logger?.LogDebug($"Obtendo objeto da fila {this._nomeFila} em {sw.ElapsedMilliseconds}ms.");

            action(JsonConvert.DeserializeObject<TEntity>(Encoding.UTF8.GetString(message.Body)));
        }
    }
}