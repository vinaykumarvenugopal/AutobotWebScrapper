using AutobotWebScrapper.Application;
using AutobotWebScrapper.Application.Interfaces.Infrastructure;
using AutobotWebScrapper.Infrastructure.NSEDataScrapper.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(WebscrapperFunctionApp.Startup))]
namespace WebscrapperFunctionApp
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient("NSEClient", client =>
            {
                client.BaseAddress = new Uri("https://www.nseindia.com");
                client.Timeout = new TimeSpan(0, 0, 30);
                client.DefaultRequestHeaders.Clear();
            });

            builder.Services.AddApplication();
            builder.Services.AddTransient<IFetchFuturesDataService, FetchFuturesDataService>();

        }
    }
}
