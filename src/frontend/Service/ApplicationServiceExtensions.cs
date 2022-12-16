using Microsoft.Extensions.DependencyInjection;
using WeeControl.Common.SharedKernel;
using WeeControl.Frontend.AppService.AppInterfaces;
using WeeControl.Frontend.AppService.AppServices;
using WeeControl.Frontend.AppService.Contexts.Business.Elevator;
using WeeControl.Frontend.AppService.Contexts.Essential.Interfaces;
using WeeControl.Frontend.AppService.Contexts.Essential.Services;
using WeeControl.Frontend.AppService.Interfaces;
using WeeControl.Frontend.AppService.Services;

namespace WeeControl.Frontend.AppService;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddUserSecurityService();
        
        services.AddSingleton<IPersistedLists, PersistedListService>();
        services.AddSingleton<IDatabaseService, DatabaseService>();
        services.AddSingleton<IDeviceSecurity, SecurityService>();
        
        //services.AddSingleton<IDevice, DeviceService>();
        
        services.AddTransient<IServerOperation, ServerOperationService>();
        
        services.AddTransient<IAuthorizationService, AuthorizationService>();
        services.AddTransient<IHomeService, HomeService>();
        
        services.AddTransient<IUserService, UserService>();
        
        services.AddTransient<TerritoryService>();

        return services;
    }
}