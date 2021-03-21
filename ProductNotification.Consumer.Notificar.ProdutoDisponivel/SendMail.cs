using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductNotification.Domain.Entities;
using ProductNotification.Domain.Interfaces.Repository;
using ProductNotification.Infrastructure.Data.MongoDB;
using System.Threading.Tasks;

namespace ProductNotification.Consumer.Notificar.ProdutoDisponivel
{
    public class SendMail
    {
        private readonly INotificationRepository _notificationRepository;
        public SendMail(INotificationRepository notificationRepository)
        {
            this._notificationRepository = notificationRepository;
        }

        [FunctionName("SendMail")]
        public async Task Run([ServiceBusTrigger("notification", Connection = "ConnectionServiceBus")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"Recebendo objeto da fila: {myQueueItem}");

            var result = JsonConvert.DeserializeObject<Product>(myQueueItem);

            var notifications = await this._notificationRepository.GetByFilterAsync(result.Codigo);

            //CALL MAIL SEND METHOD

            await Task.CompletedTask;
        }
    }
}