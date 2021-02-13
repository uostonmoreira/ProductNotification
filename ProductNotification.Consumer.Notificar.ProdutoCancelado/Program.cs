using Microsoft.Extensions.DependencyInjection;
using ProductNotification.Domain.Entities;
using ProductNotification.Domain.Interfaces.Mensageria;
using ProductNotification.Infrastructure.RabbitMQ;
using System;

namespace ProductNotification.Consumer.Notificar.ProdutoDisponivel
{
    class Program
    {
        private static IMensageria<Product> _mensageria;
        private const string _filaNotificacaoProduto = "customerproductnotificationcancelado";
        static void Main(string[] args)
        {
            Console.WriteLine($"Iniciando consumer em {DateTime.Now}");
            Console.WriteLine("");

            IServiceCollection services = new ServiceCollection();
            services.AddScoped(typeof(IMensageria<>), typeof(Mensageria<>));
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            _mensageria = serviceProvider.GetService<IMensageria<Product>>();

            _mensageria.LerMensagem(_filaNotificacaoProduto, (message) =>
            {
                Console.WriteLine($"Lendo mensagem {message} em {DateTime.Now}");
                Console.WriteLine("----------------------------------------------");
            });
        }
    }
}