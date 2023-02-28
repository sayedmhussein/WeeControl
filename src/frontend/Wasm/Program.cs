using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System.Net.Http;
using System.Threading.Tasks;
using WeeControl.Core.SharedKernel;
using WeeControl.Frontend.Wasm.Services;
using WeeControl.Host.WebApiService;
using WeeControl.Host.WebApiService.DeviceInterfaces;

namespace WeeControl.Frontend.Wasm;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");

        builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
        
        builder.Services.AddWebApiService();

        builder.Services.AddUserSecurityService();

        builder.Services.AddOptions();
        builder.Services.AddAuthorizationCore();

        builder.Services.AddMudServices();

        // builder.Services.AddHttpClient("UnSecured", 
        //     client => client.BaseAddress = new Uri("https://localhost:5001/"));
        //     
        // builder.Services.AddHttpClient("Secured", 
        //         client => client.BaseAddress = new Uri("https://localhost:5001/"))
        //     .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        builder.Services.AddHttpClient("UnSecured");
        builder.Services.AddHttpClient("Secured")
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
            .CreateClient("Secured"));

        builder.Services.AddApiAuthorization(options =>
        {

            //options.AuthenticationPaths.LogInPath = ApplicationService.Pages.Essential.Authentication.LoginPage;
        });

        // builder.Services.Configure<ForwardedHeadersOptions>(options =>
        // {
        //     options.KnownProxies.Add(IPAddress.Parse("10.0.0.100"));
        // });

        builder.Services.AddSingleton<DeviceDataService>();
        builder.Services.AddSingleton<ICommunication>(x => x.GetRequiredService<DeviceDataService>());
        builder.Services.AddSingleton<IFeature>(x => x.GetRequiredService<DeviceDataService>());
        builder.Services.AddSingleton<IGui, GuiService>();
        builder.Services.AddSingleton<IMedia>(x => x.GetRequiredService<DeviceDataService>());
        builder.Services.AddSingleton<ISharing>(x => x.GetRequiredService<DeviceDataService>());
        builder.Services.AddSingleton<IStorage>(x => x.GetRequiredService<DeviceDataService>());

        await builder.Build().RunAsync();
    }
}