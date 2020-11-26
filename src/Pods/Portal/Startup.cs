using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.SignalRBench.Common;
using Azure.SignalRBench.Coordinator;
using Azure.SignalRBench.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Portal.Controllers;
using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.FileProviders;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using Constants=Azure.SignalRBench.Common.Constants;

namespace Portal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // AAD auth
            
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAd"));
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });
            services
                .AddRazorPages()
                .AddMicrosoftIdentityUI();

         
            services.AddSingleton(
                       sp => new SecretClient(
                           new Uri(Configuration[Constants.ConfigurationKeys.KeyVaultUrlKey]),
                           new DefaultAzureCredential()));
            services.AddSingleton<IPerfStorage>(sp =>
            {
                var secretClient = sp.GetService<SecretClient>();
                var connectionString = secretClient.GetSecretAsync(Constants.KeyVaultKeys.StorageConnectionStringKey).GetAwaiter().GetResult().Value.Value;
                return new PerfStorage(connectionString);
            }
                );
            services.AddControllersWithViews().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
              //  options.JsonSerializerOptions.IgnoreNullValues =true;
            });
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        
        app.UseHttpsRedirection();
        
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSpaStaticFiles();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapRazorPages();
            //endpoints.MapControllerRoute("testconfig", "testconfig/{action}");
            //endpoints.MapControllerRoute("teststatus", "teststatus/{action}");
        
            // endpoints.MapControllerRoute(
            //     name: "default",
            //     pattern: "{controller}/{action=InstanceIndex}/{id?}");
        });
        
        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp";
        
            if (env.IsDevelopment())
            {
                spa.UseReactDevelopmentServer(npmScript: "start");
            }
        });
        }
    }
}
