using RabbitMQ.Client;
using System;

namespace ProductNotification.Domain.Interfaces.Mensageria
{
    public interface IMensageria<TEntity>
    {
        public void EnviaMensagem(TEntity obj, string fila);
        public void LerMensagem(string fila, Action<string> action);
    }
}
