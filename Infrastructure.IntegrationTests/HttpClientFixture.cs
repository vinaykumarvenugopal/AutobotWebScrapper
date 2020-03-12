using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Infrastructure.IntegrationTests
{
    public class HttpClientFixture : IDisposable
    {
        public IHttpClientFactory HTTPClientFactory { get; private set; }
        
        public HttpClientFixture()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddHttpClient("NSEClient", client =>
            {
                client.BaseAddress = new Uri("https://www.nseindia.com");
                client.Timeout = new TimeSpan(0, 0, 30);
                client.DefaultRequestHeaders.Clear();
            });
      
            HTTPClientFactory = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();
        }
        public void Dispose()
        {
          
        }
    }
}
