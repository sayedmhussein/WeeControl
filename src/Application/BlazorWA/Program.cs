using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WeeControl.SharedKernel.BasicSchemas.Common.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Common.Enums;
using WeeControl.Applications.BaseLib.Interfaces;
using WeeControl.Applications.BaseLib.Services;

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
                client.BaseAddress = new Uri(new ApiDicts().ApiRoute[ApiRouteEnum.Base]));

            //IDevice device;
            builder.Services.AddTransient<IDevice>(null);
            //builder.Services.AddTransient<IServerService>(new ServerService(null, null, null, null));

            await builder.Build().RunAsync();
        }
    }
}
