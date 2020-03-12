using AutobotWebScrapper.Application;
using AutobotWebScrapper.Application.Interfaces.Infrastructure;
using AutobotWebScrapper.Infrastructure.NSEDataScrapper.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TestAPICalls
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                var config = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json", optional: false,
                                                reloadOnChange: true)
                                 .Build();
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection, config);
                var serviceProvider = serviceCollection.BuildServiceProvider();

                using (serviceProvider as IDisposable)
                {
                    var entryPoint = serviceProvider.GetRequiredService<IEntryPointService>();

                    await entryPoint.RunAsync();


                    Console.WriteLine(" Press [enter] to exit.");

                    Console.ReadLine();
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");

                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }
        private static void ConfigureServices(IServiceCollection serviceCollection,
                                    IConfiguration config)
        {
            serviceCollection.AddLogging(loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog(config);
            });

      
            serviceCollection.AddHttpClient("NSEClient", client =>
            {
                client.BaseAddress = new Uri("https://www.nseindia.com");
                client.Timeout = new TimeSpan(0, 0, 30);
                client.DefaultRequestHeaders.Clear();
            });

            serviceCollection.AddApplication();
            serviceCollection.AddTransient<IEntryPointService, EntryPointService>();
            serviceCollection.AddTransient<IFetchFuturesDataService, FetchFuturesDataService>();
        }
    }
}
