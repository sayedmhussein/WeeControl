using Microsoft.Extensions.DependencyInjection;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Services;
using WeeControl.Frontend.AppService.Contexts.Business.Elevator;
using WeeControl.Frontend.AppService.Contexts.Essential.Interfaces;
using WeeControl.Frontend.AppService.Contexts.Essential.Services;
using WeeControl.Frontend.AppService.Interfaces;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;
using WeeControl.Frontend.AppService.Services;

namespace WeeControl.Frontend.AppService;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IJwtService, JwtService>();
        
        services.AddSingleton<IPersistedLists, PersistedListService>();
        services.AddSingleton<IDatabaseService, DatabaseService>();
        
        services.AddSingleton<IDevice, DeviceService>();
        
        services.AddTransient<IServerOperation, ServerOperationService>();
        
        services.AddTransient<IUserAuthorizationService, UserAuthorizationService>();
        services.AddTransient<IUserService, UserService>();
        
        services.AddTransient<TerritoryService>();

        return services;
    }
}