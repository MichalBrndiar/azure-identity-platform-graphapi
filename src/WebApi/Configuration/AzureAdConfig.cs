using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Configuration
{
    public class AzureAdConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
        public string Instance { get; set; } = "https://login.microsoftonline.com";
        public string Authority => $"{Instance}{TenantId}";
    }
}
