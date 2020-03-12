
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using AutobotWebScrapper.Application.Interfaces.Infrastructure;
using AutobotWebScrapper.Infrastructure.NSEDataScrapper.Services;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.IntegrationTests
{
    public class StartupFixture : IDisposable
    {
        IServiceCollection services;
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        public StartupFixture()
        {
            services = new ServiceCollection();
            services.AddHttpClient("NSEClient", client =>
            {
                client.BaseAddress = new Uri("https://www.nseindia.com");
                client.Timeout = new TimeSpan(0, 0, 30);
                client.DefaultRequestHeaders.Clear();
            });
            services.AddScoped<IFetchFuturesDataService, FetchFuturesDataService>();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
