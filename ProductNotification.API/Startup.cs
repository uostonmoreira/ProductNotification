using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ProductNotification.API.Extensions;
using ProductNotification.Infrastructure.Data.Context;
using ProductNotification.Infrastructure.IoC;
using ProductNotification.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;

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
            .AddApplicationInsights()
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

            services.AddSingleton(logger);
            services.AddSingleton(loggerFactory);

            services.AddControllers();
            services.AddHealthChecks()
                .AddDbContextCheck<ContextDB>();

            services.AddApplicationInsightsTelemetry();

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

            app.UseHealthChecks("/ProductNotification/check",
              new HealthCheckOptions()
              {
                  ResponseWriter = async (context, report) =>
                  {
                      var result = JsonConvert.SerializeObject(
                          new
                          {
                              statusApplication = report.Status.ToString(),
                              healthChecks = report.Entries.Select(e => new
                              {
                                  check = e.Key,
                                  ErrorMessage = e.Value.Exception?.Message,
                                  status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                              })
                          });
                      context.Response.ContentType = MediaTypeNames.Application.Json;
                      await context.Response.WriteAsync(result);
                  }
              });

            //Global Exception Handler
            app.ConfigureExceptionHandler(logger);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecksUI();
            });
        }
    }
}