using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using System.Net.Http;
using System.Threading.Tasks;
using WeeControl.Core.SharedKernel;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.Wasm.Services;

namespace WeeControl.Frontend.Wasm;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<App>("#app");

        builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

        builder.Services.AddSingleton<IDeviceData, DeviceDataService>();

        builder.Services.AddUserSecurityService();
        builder.Services.AddApplicationServices();

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

        await builder.Build().RunAsync();
    }
}