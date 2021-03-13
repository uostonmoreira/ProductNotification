using System;
using System.Threading.Tasks;

namespace ProductNotification.Domain.Interfaces.Mensageria
{
    public interface IMensageria
    {
        Task SendMessageAsync<TEntity>(TEntity entity);
        Task ReceiveMessagesAsync<TEntity>(Action<TEntity> action);
    }
}
