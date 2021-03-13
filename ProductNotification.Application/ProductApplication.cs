using ProductNotification.Application.Interfaces;
using ProductNotification.Domain.Entities;
using ProductNotification.Domain.Interfaces.Mensageria;
using ProductNotification.Domain.Interfaces.Repository;
using System;
using System.Threading.Tasks;

namespace ProductNotification.Application
{
    public class ProductApplication : IProductApplication
    {
        private readonly IProductRepository _productRepository;
        private readonly IMensageria _mensageria;

        public ProductApplication(IProductRepository productRepository, IMensageriaServiceBus mensageriaServiceBus)
        {
            this._productRepository = productRepository;
            this._mensageria = mensageriaServiceBus;
        }

        public async Task<int> InsertAsync(Product product)
        {
            int result = await this._productRepository.InsertAsync(product);

            if (result == 1)
                await this._mensageria.SendMessageAsync<Product>(product);

            return result;

            /*
            await this._mensageria.ReceiveMessagesAsync<Product>((message) =>
            {
                Console.WriteLine($"Name: {message.Name}");
            });
            */
        }
    }
}
