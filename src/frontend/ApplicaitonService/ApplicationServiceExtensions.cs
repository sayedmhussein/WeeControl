using Microsoft.Extensions.DependencyInjection;
using WeeControl.Frontend.ApplicationService.Contexts.Essential.Interfaces;
using WeeControl.Frontend.ApplicationService.Contexts.Essential.ViewModels;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.Frontend.ApplicationService.Services;

namespace WeeControl.Frontend.ApplicationService;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<IPersistedLists, PersistedListService>();
        services.AddTransient<IServerOperation, ServerOperationService>();
        
        services.AddTransient<IAuthorizationViewModel, AuthorizationViewModel>();
        services.AddTransient<IUserViewModel, UserViewModel>();
        services.AddTransient<IHomeViewModel, HomeViewModel>();
        
        
        services.AddTransient<TerritoryViewModel>();

        return services;
    }
}