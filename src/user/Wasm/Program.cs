using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.User.UserServiceCore.Interfaces;
using WeeControl.User.Wasm.Services;
using SharedDependency = WeeControl.SharedKernel.DependencyInjection;
using UserServiceDependency = WeeControl.User.UserServiceCore.DependencyExtension;

namespace WeeControl.User.Wasm;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        
        builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
        
        builder.Services.AddScoped<IDevice, DeviceService>();
        builder.Services.AddScoped<IDeviceAlert, DeviceAlertSimple>();
        builder.Services.AddScoped<IDeviceLocation, DeviceLocationService>();
        builder.Services.AddScoped<IDevicePageNavigation, DevicePageNavigationService>();
        builder.Services.AddScoped<IDeviceServerCommunication, EssentialDeviceServerDeviceService>();
        builder.Services.AddScoped<IDeviceStorage, DeviceStorageService>();
        
        //builder.Services.AddScoped<IDeviceSecurity>(p => p.GetService<AuthStateProvider>());
        builder.Services.AddScoped<IDeviceSecurity, AuthStateProvider>();

        SharedDependency.AddUserSecurityServiceForApplication(builder.Services);
        UserServiceDependency.AddUserServiceCore(builder.Services);

        builder.Services.AddOptions();

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