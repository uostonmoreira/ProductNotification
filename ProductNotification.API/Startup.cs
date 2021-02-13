using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProductNotification.API.Extensions;
using ProductNotification.Infrastructure.IoC;
using ProductNotification.Infrastructure.Utility;
using System.Collections.Generic;
using System.IO;

namespace ProductNotification.API
{
    public class Startup
    {
        private ILoggerFactory loggerFactory { get; }
        private ILogger logger { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            loggerFactory = LoggerFactory.Create(opt =>
            opt
            .AddConsole()
            .AddDebug()
            .AddConfiguration(Configuration.GetSection("Logging")));

            logger = loggerFactory.CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var environment = Global.ASPNETCORE_ENVIRONMENT;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: false)
                .AddEnvironmentVariables()
                .Build();

            services.AddControllers();

            services.AddDependencies(configuration);

            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = $"Cadastro de Produto - API {environment}", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //Exibe o swagger somente nos ambientes não produtivos
            if (!env.EnvironmentName.Equals("Production"))
            {
                app.UseSwagger(c =>
                {
                    if (!env.IsDevelopment())
                    {
                        c.RouteTemplate = "swagger/{documentName}/swagger.json";

                        var basepath = "/ProdutoApi/";
                        c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                        {
                            IDictionary<string, OpenApiPathItem> paths = new Dictionary<string, OpenApiPathItem>();
                            OpenApiPaths keyValuePairs = new OpenApiPaths();
                            foreach (var path in swaggerDoc.Paths)
                            {
                                keyValuePairs.Add(path.Key.Replace("/", basepath), path.Value);
                            }

                            swaggerDoc.Paths = keyValuePairs;
                        });
                    }
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("./v1/swagger.json", "API de Produto");
                    c.RoutePrefix = "swagger";
                });
            }

            //Global Exception Handler
            app.ConfigureExceptionHandler(logger);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}