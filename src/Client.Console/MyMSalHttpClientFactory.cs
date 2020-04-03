using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Console
{
    public class MyMSalHttpClientFactory : IMsalHttpClientFactory
    {
        private readonly IHttpClientFactory clientFactory;

        public MyMSalHttpClientFactory(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public HttpClient GetHttpClient()
        {
            return this.clientFactory.CreateClient();
        }
    }
}
