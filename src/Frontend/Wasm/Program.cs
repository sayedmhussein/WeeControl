using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

            builder.Services.AddCommonLibraryService();
            builder.Services.AddSingleton<ToastService>();
            
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5001/") });

            builder.Services.AddSingleton<ILocalStorage, LocalStorageService>();
            builder.Services.AddTransient<IDevice, DeviceService>();
            
            builder.Services.AddHttpClient(IHttpService.UnSecuredApi, 
                client => client.BaseAddress = new Uri("https://localhost:5001/"));
            
            builder.Services.AddHttpClient(IHttpService.SecuredApi, 
                    client => client.BaseAddress = new Uri("https://localhost:5001/"))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient(IHttpService.SecuredApi));
            
            await builder.Build().RunAsync();
        }
    }
}
