using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Frontend.Wasm.Services;
using DependencyInjection = WeeControl.Common.UserSecurityLib.DependencyInjection;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Frontend.ServiceLibrary.Operations;
using WeeControl.Frontend.Wasm.Interfaces;

namespace WeeControl.Frontend.Wasm
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddTransient<IUserDevice, UserDeviceService>();
            builder.Services.AddScoped<IUserStorage, UserStorageService>();
            builder.Services.AddScoped<IUserCommunication>(provider =>
            {
                var factory = provider.GetService<IHttpClientFactory>();
                return new UserCommunicationService(factory);
            });
            
            DependencyInjection.AddUserSecurityService(builder.Services);
            

            builder.Services.AddScoped<Common.BoundedContext.Credentials.IUserOperation>(provider =>
            {
                var device = provider.GetService<IUserDevice>();
                var factory = provider.GetService<IUserCommunication>();
                return new CredentialsOperation(device, factory);
            });

            builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            builder.Services.AddScoped<ISecurityService, SecurityService>(); ///////////////////////////////////////////
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
