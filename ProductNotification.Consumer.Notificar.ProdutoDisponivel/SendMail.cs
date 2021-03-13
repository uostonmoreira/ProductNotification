using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ProductNotification.Consumer.Notificar.ProdutoDisponivel
{
    public static class SendMail
    {
        [FunctionName("SendMail")]
        public static void Run([ServiceBusTrigger("notification", Connection = "ConnectionServiceBus")] string myQueueItem, ILogger log)
        {
            log.LogInformation($"Recebendo objeto da fila: {myQueueItem}");

            //Faça algo com o resultado da fila
        }
    }
}