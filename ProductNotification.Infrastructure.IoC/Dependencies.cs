using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductNotification.Application;
using ProductNotification.Application.Interfaces;
using ProductNotification.Domain.Interfaces.Mensageria;
using ProductNotification.Domain.Interfaces.Repository;
using ProductNotification.Infrastructure.Data.Context;
using ProductNotification.Infrastructure.Data.Repositories;
using ProductNotification.Infrastructure.RabbitMQ;
using ProductNotification.Infrastructure.ServiceBus;

namespace ProductNotification.Infrastructure.IoC
{
    public static class Dependencies
    {
        public static void AddDependencies(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddSingleton<IConfiguration>(x => configuration);

            // Services
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductApplication, ProductApplication>();
            services.AddDbContext<ContextDB>(opt => opt.UseInMemoryDatabase("MockDb"));

            // Mensageria
            services.AddScoped<IMensageriaRabbitMQ, MensageriaRabbitMQ>();
            services.AddScoped<IMensageriaServiceBus, MensageriaServiceBus>();
        }
    }
}
