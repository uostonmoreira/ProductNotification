﻿using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductNotification.Consumer.Notificar.ProdutoDisponivel;
using ProductNotification.Domain.Interfaces.Repository;
using ProductNotification.Infrastructure.Data.MongoDB;
using ProductNotification.Infrastructure.Utility;

[assembly: WebJobsStartup(typeof(Startup))]
namespace ProductNotification.Consumer.Notificar.ProdutoDisponivel
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
			var environment = Global.ASPNETCORE_ENVIRONMENT;
			
            var config = new ConfigurationBuilder()
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
			//.AddJsonFile($"appsettings.{environment}.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

            builder.Services.AddSingleton<IMongoDB>(x =>
            {
                var _configuration = x.GetService<IConfiguration>();
                return new Infrastructure.Data.MongoDB.MongoDB(config["ConnectionStringMongoDB"], config["DataBaseMongoDB"]);
            });
            builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
        }
    }
}
