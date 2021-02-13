using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductNotification.Domain.Interfaces.Mensageria;
using ProductNotification.Domain.Interfaces.Repository;
using ProductNotification.Infrastructure.Data.Context;
using ProductNotification.Infrastructure.Data.Repositories;
using ProductNotification.Infrastructure.RabbitMQ;

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
            services.AddDbContext<ContextDB>(opt => opt.UseInMemoryDatabase("MockDb"));

            // Mensageria
            services.AddSingleton(typeof(IMensageria<>), typeof(Mensageria<>));
        }
    }
}
