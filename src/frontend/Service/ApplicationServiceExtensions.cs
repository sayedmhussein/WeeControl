using Microsoft.Extensions.DependencyInjection;
using WeeControl.Frontend.Service.Contexts.Business.Elevator;
using WeeControl.Frontend.Service.Contexts.Essential.Interfaces;
using WeeControl.Frontend.Service.Contexts.Essential.Services;
using WeeControl.Frontend.Service.Interfaces;
using WeeControl.Frontend.Service.Services;

namespace WeeControl.Frontend.Service;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IPersistedLists, PersistedListService>();
        services.AddTransient<IServerOperation, ServerOperationService>();
        
        services.AddTransient<IUserAuthorizationService, UserAuthorizationService>();
        services.AddTransient<IUserService, UserService>();
        
        services.AddTransient<TerritoryService>();

        return services;
    }
}