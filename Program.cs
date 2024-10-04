using System.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using ILogger = NLog.ILogger;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Net.Http.Headers;
using NLog.Extensions.Logging;
using AgileActors.AggregationApp.Types.Context;

namespace Portfolio.Core
{
    public static class Program
    {
        #region Main Functionality
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
            try
            {
                logger.Debug("Welcome Aboard!");
                return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                    .ConfigureServices((hostContext, services) =>
                    {
                        // Register Basic-Default Services:                        
                        hostContext.ConfigureIConfigurationService(services, out var config);

                        // Register Controllers Middleware:
                        services.AddControllers().AddJsonOptions(opts =>
                        {
                            opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                        });

                        // Register Http Clients,
                        services.RegisterHttpClients(config);

                        // Register your services and interfaces here                        
                        services.RegisterServices(logger);

                        // Register the Swagger generator
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Portfolio.Core", Version = "v1" });
                        });
                    })
                    .ConfigureLogging(logging =>
                    {
                        // logging.ClearProviders(); -> Not letting Running the App
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                        logging.AddNLog(); // Make sure NLog is correctly set up
                    })
                    .Configure(app =>
                    {
                        app.UseHttpsRedirection();

                        app.UseRouting();

                        app.UseSwagger();

                        app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio.Core.V1");
                        });

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    })
                    .UseUrls("http://*:5123");
                });
            }
            catch (Exception ex)
            {
                logger.Error(ex?.Message, "Stopped program because of exception");
                throw new Exception("Program" + ex?.Message);
            }
            finally
            {
                NLog.LogManager.Shutdown();
            }
        }
        #endregion

        #region Configuration Settings Methods
        private static void ConfigureIConfigurationService(this WebHostBuilderContext hostContext, IServiceCollection services, out IConfigurationRoot config)
        {
            if (hostContext.HostingEnvironment.IsDevelopment() || hostContext.HostingEnvironment.IsProduction())
            {
                // Load configuration based on the environment
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                configuration = new ConfigurationBuilder()
                    .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{configuration["ASPNETCORE_ENVIRONMENT"] ?? "Development"}.json", optional: true, reloadOnChange: true)
                    .Build();

                config = configuration;

                // Bind configuration to AppSettings class and add it to the service collection                
                services.Configure<AppSettings>(settings =>
                {
                    settings.AllowedHosts = configuration.ConfigureBasicSettings();
                    settings.ConnectionStrings = configuration.ConfigureConnectionStrings();
                    settings.ExternalApiSettings = configuration.ConfigureExternalApiSettings();
                });
            }
            else
            {
                throw new Exception("Error on initializing the Configuration Service");
            }
        }

        private static string ConfigureBasicSettings(this IConfiguration configuration)
        {
            return configuration["AllowedHosts"] ?? "";
        }

        private static ConnectionStrings ConfigureConnectionStrings(this IConfiguration configuration)
        {
            return new ConnectionStrings
            {
                IdentityConnection = configuration["ConnectionStrings:MySqlConnection"] ?? ""
            };
        }

        private static ExternalApiSettings ConfigureExternalApiSettings(this IConfiguration configuration)
        {
            var externalApiSettings = new ExternalApiSettings();
            configuration.GetSection("ExternalApis").Bind(externalApiSettings.ExternalApiSettingsList);
            return externalApiSettings;
        }
        #endregion

        #region Services Injection
        private static void RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
        }

        private static void RegisterServices(this IServiceCollection services, Logger logger)
        {
            // Singleton Lifecycle:
            // --------------------------------------------------------------------
            services.AddSingleton<ILogger, Logger>(x => logger);

            // Scoped Lifecycle:
            // --------------------------------------------------------------------


            // Transient Lifecycle:
            // --------------------------------------------------------------------

        }
        #endregion

    }
}