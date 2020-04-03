using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Client.Console
{

    public class AzureAdConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public string Instance { get; set; } = "https://login.microsoftonline.com";
        public string Authority => $"{Instance}{TenantId}";

        public IEnumerable<string> Scopes { get; set; }
    }
}
