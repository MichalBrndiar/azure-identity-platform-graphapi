using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;

using WebApi.Configuration;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddAzureAD(options => Configuration.Bind("AzureAd", options));

            //services.Configure<JwtBearerDefaults>(JwtBearerDefaults.AuthenticationScheme, options =>
            //{
            //    options.Authority = options.Authority + "/v2.0/";         // Microsoft identity platform

            //    options.TokenValidationParameters.ValidateIssuer = false; // accept several tenants (here simplified)
            //});

            services.Configure<AzureAdConfig>(this.Configuration.GetSection("AzureAd"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddProtectedWebApi(this.Configuration, "AzureAd");

            services.AddProtectedWebApiCallsProtectedWebApi(this.Configuration, "AzureAd")
                    .AddInMemoryTokenCaches();

            services.AddHttpClient();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

                // Since IdentityModel version 5.2.1 (or since Microsoft.AspNetCore.Authentication.JwtBearer version 2.2.0),
                // PII hiding in log files is enabled by default for GDPR concerns.
                // For debugging/development purposes, one can enable additional detail in exceptions by setting IdentityModelEventSource.ShowPII to true.
                // Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
                Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
