using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client.Console
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            using var provider = BuildServiceProvider();

            var factory = provider.GetService<IMsalHttpClientFactory>();
            var config = provider.GetService<IOptionsMonitor<AzureAdConfig>>().CurrentValue;
            var app = ConfidentialClientApplicationBuilder
                .Create(config.ClientId)
                .WithHttpClientFactory(factory)
                .WithClientSecret(config.ClientSecret)
                .WithAuthority(new Uri(config.Authority))
                .Build();

            var result = await app.AcquireTokenForClient(config.Scopes)
                  .ExecuteAsync();

            var client = provider.GetService<IHttpClientFactory>().CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            var response = await client.GetStringAsync("https://localhost:5001/api/values/graph");

            System.Console.WriteLine(response);
        }

        private static ServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();

            var config = new ConfigurationBuilder()
                .AddJsonFile("appconfig.json", optional: true)
                .Build();

            services.Configure<AzureAdConfig>(config.GetSection("AzureAd"));
            services.AddLogging(options =>
            {
                options.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                options.AddConsole();
            });

            services.AddHttpClient();
            services.AddSingleton<IMsalHttpClientFactory, MyMSalHttpClientFactory>();

            return services.BuildServiceProvider();
        }
    }
}
