using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using WeeControl.Common.SharedKernel;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.Authentication;
using WeeControl.Common.SharedKernel.BoundedContexts.HumanResources.ClientSideServices;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Frontend.Wasm.Interfaces;
using WeeControl.Frontend.Wasm.Services;
using DependencyInjection = WeeControl.Common.UserSecurityLib.DependencyInjection;

namespace WeeControl.Frontend.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped<ILocalStorage, LocalStorageService>();
            builder.Services.AddTransient<IClientDevice, DeviceService>();
            
            
            DependencyInjection.AddUserSecurityService(builder.Services);
            

            builder.Services.AddScoped<IAuthenticationService>(provider =>
            {
                var device = provider.GetService<IClientDevice>();
                var factory = provider.GetService<IHttpClientFactory>();
                
                return new AuthenticationService(factory, device);
            });

            //builder.Services.AddTransient<IAuthenticationRefresh, AuthenticationRefreshService>();
            
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            //builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            
            
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });


            ApiRouteLink.HumanResources.Base = "https://localhost:5001/";

            builder.Services.AddHttpClient("UnSecured", 
                client => client.BaseAddress = new Uri("https://localhost:5001/"));
            
            builder.Services.AddHttpClient("Secured", 
                    client => client.BaseAddress = new Uri("https://localhost:5001/"))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("Secured"));

            
            // builder.Services.AddAuthorizationCore(o =>
            // {
            //     o.AddPolicy("SuperAdmin", policy => policy.RequireClaim("SuperAdmin"));
            //     o.AddPolicy("CountyAdmin", policy => policy.RequireClaim("CountyAdmin"));
            // });

            builder.Services.AddApiAuthorization(options =>
            {
                options.AuthenticationPaths.LogInPath = "login";
            });
            
            await builder.Build().RunAsync();
        }
    }
}
