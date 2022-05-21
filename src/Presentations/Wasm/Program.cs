using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Presentations.ServiceLibrary.EssentialContext;
using WeeControl.Presentations.ServiceLibrary.Interfaces;
using WeeControl.Presentations.Wasm.Services;
using WeeControl.SharedKernel.Essential;
using DependencyInjection = WeeControl.SharedKernel.DependencyInjection;

namespace WeeControl.Presentations.Wasm;

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
        builder.Services.AddScoped<IDevicePageNavigation, DevicePageNavicationService>();
        builder.Services.AddScoped<IDeviceServerCommunication, EssentialDeviceServerDeviceService>();
        builder.Services.AddScoped<IDeviceStorage, DeviceStorageService>();
        
        //builder.Services.AddScoped<IDeviceSecurity>(p => p.GetService<AuthStateProvider>());
        builder.Services.AddScoped<IDeviceSecurity, AuthStateProvider>();

        DependencyInjection.AddUserSecurityServiceForApplication(builder.Services);
        
        
        builder.Services.AddScoped<IUserOperation, UserOperation>();
        
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