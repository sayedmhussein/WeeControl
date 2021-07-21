using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Common;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;

namespace BlazorWA
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddHttpClient("WebAPI", client =>
                client.BaseAddress = new Uri(new CommonLists().GetRoute(ApiRouteEnum.Base)));

            //IDevice device;
            builder.Services.AddTransient<IDevice>(null);
            //builder.Services.AddTransient<IServerService>(new ServerService(null, null, null, null));

            await builder.Build().RunAsync();
        }
    }
}
