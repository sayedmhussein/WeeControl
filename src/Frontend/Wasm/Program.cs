using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Frontend.CommonLib;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using WeeControl.Frontend.CommonLib.Interfaces;
using WeeControl.Frontend.Wasm.Services;

namespace WeeControl.Frontend.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            
            builder.RootComponents.Add<App>("#app");
            
            builder.Services.AddScoped<ILocalStorage, LocalStorageService>();

            builder.Services.AddTransient<IAuthenticationRefresh, AuthenticationRefreshService>();
            
            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddCommonLibraryService();

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddAuthorizationCore();
            
            
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });

            
            builder.Services.AddTransient<IDevice, DeviceService>();

            builder.Services.AddHttpClient(IHttpService.UnSecuredApi, 
                client => client.BaseAddress = new Uri("https://localhost:5001/"));
            
            builder.Services.AddHttpClient(IHttpService.SecuredApi, 
                    client => client.BaseAddress = new Uri("https://localhost:5001/"))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient(IHttpService.SecuredApi));

            
            builder.Services.AddAuthorizationCore(o =>
            {
                
                o.AddPolicy("SuperAdmin", policy => policy.RequireClaim("SuperAdmin"));
                o.AddPolicy("CountyAdmin", policy => policy.RequireClaim("CountyAdmin"));
            });

            builder.Services.AddApiAuthorization(options =>
            {
                options.AuthenticationPaths.LogInPath = "login";
            });
            
            await builder.Build().RunAsync();
        }
    }
}
