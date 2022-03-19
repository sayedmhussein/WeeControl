using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Frontend.Wasm.Interfaces;
using WeeControl.Frontend.Wasm.Services;
using DependencyInjection = WeeControl.Common.UserSecurityLib.DependencyInjection;
using WeeControl.Common.BoundedContext.Credentials.Operations;
using WeeControl.Common.BoundedContext.Interfaces;

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
            builder.Services.AddTransient<IUserDevice, DeviceService>();
            
            DependencyInjection.AddUserSecurityService(builder.Services);
            

            builder.Services.AddScoped<IUserOperation>(provider =>
            {
                var device = provider.GetService<IUserDevice>();
                var factory = provider.GetService<IHttpClientFactory>();
                return new UserOperation(device, factory);
            });

            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            
            builder.Services.AddAuthorizationCore();

            builder.Services.AddHttpClient("UnSecured", 
            client => client.BaseAddress = new Uri("https://localhost:5001/"));
            
            builder.Services.AddHttpClient("Secured", 
                    client => client.BaseAddress = new Uri("https://localhost:5001/"))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("Secured"));
            
            builder.Services.AddApiAuthorization(options =>
            {
                options.AuthenticationPaths.LogInPath = "login";
            });
            
            await builder.Build().RunAsync();
        }
    }
}
