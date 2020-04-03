using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using WebApi.Configuration;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> log;
        private readonly ITokenAcquisition tokenAcquisition;
        private readonly IHttpClientFactory clientFactory;
        private readonly AzureAdConfig config;

        public ValuesController(
            ILogger<ValuesController> log,
            IOptionsMonitor<AzureAdConfig> config,
            ITokenAcquisition tokenAcquisition,
            IHttpClientFactory clientFactory)
        {
            this.log = log;
            this.tokenAcquisition = tokenAcquisition;
            this.clientFactory = clientFactory;
            this.config = config.CurrentValue;
        }

        /// <summary>
        /// Returns sample data to verify that service is up and running.
        /// </summary>
        /// <returns></returns>
        [HttpGet("nograph")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return this.Ok(new[]
            {
                "Hello",
                "Dolly"
            });
        }

        /// <summary>
        /// Invokes Graph API to retrieve users in tenant's directory.
        /// Uses application token (no client in token is passed) in current implementation.
        /// </summary>
        /// <returns></returns>
        [HttpGet("graph")]
        public async Task<ActionResult<IEnumerable<string>>> GetFromGraph()
        {
            // No identity in current implementation
            this.log.LogInformation($"Current user {this.User.Identity.Name}");

            var app = ConfidentialClientApplicationBuilder
                       .Create(this.config.ClientId)
                       .WithAuthority(AzureCloudInstance.AzurePublic, this.config.TenantId)
                       .WithClientSecret(this.config.ClientSecret)
                       .Build();

            //app.AcquireTokenOnBehalfOfAsync()
            var token = await app.AcquireTokenForClient(new[] { "https://graph.microsoft.com/.default" }).ExecuteAsync();

            //var token = await this.tokenAcquisition.GetAccessTokenForUserAsync(new[] { "User.Read.All" });
            //var token = await this.tokenAcquisition.AcquireTokenForAppAsync(new[] { "https://graph.microsoft.com/.default" });

            this.log.LogWarning($"Received token: {token.AccessToken}");
            var result = await this.CallGraphApi(token.AccessToken);

            // Do whatever you want with retrieved data
            // It should be good to convert result from Graph API invocation to POCO objects

            // Serializing to formatted JSON just for sake of nice output in Client.Console :)
            return Ok(JsonConvert.SerializeObject(result, Formatting.Indented));
        }

        /// <summary>
        /// Calls the Graph API 'users' endpoint.
        /// </summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns>Retrieved data as <see cref="dynamic"/></returns>
        private async Task<dynamic> CallGraphApi(string accessToken)
        {
            var client = this.clientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://graph.microsoft.com/v1.0/users");
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Deserialization should be performed to some POCO objects
                return JsonConvert.DeserializeObject(content);
            }

            throw new Exception(content);
        }
    }
}
