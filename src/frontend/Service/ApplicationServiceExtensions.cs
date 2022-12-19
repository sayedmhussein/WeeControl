using Microsoft.Extensions.DependencyInjection;
using WeeControl.Common.SharedKernel;
using WeeControl.Frontend.AppService.DeviceInterfaces;
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;
using WeeControl.Frontend.AppService.GuiInterfaces.Home;
using WeeControl.Frontend.AppService.Internals.Interfaces;
using WeeControl.Frontend.AppService.Internals.Services;

namespace WeeControl.Frontend.AppService;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddUserSecurityService(); //From Shared Kernel

        services.AddSingleton<IStorage>(x => x.GetRequiredService<IDeviceData>());

        services.AddSingleton<IPersistedLists, PersistedListService>();
        services.AddSingleton<IDatabaseService, DatabaseService>();
        services.AddSingleton<IDeviceSecurity, SecurityService>();
        services.AddSingleton<IServiceData, ServiceDataService>();

        services.AddTransient<IServerOperation, ServerOperationService>();
        
        services.AddTransient<IAuthorizationService, AuthorizationService>();
        services.AddTransient<IHomeService, HomeService>();
        
        //services.AddTransient<IUserService, UserService>();
        
        //services.AddTransient<TerritoryService>();

        return services;
    }
}