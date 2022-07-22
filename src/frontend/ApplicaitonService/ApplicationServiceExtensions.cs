using Microsoft.Extensions.DependencyInjection;
using WeeControl.Frontend.ApplicationService.Essential.Authorization;
using WeeControl.Frontend.ApplicationService.Essential.Home;
using WeeControl.Frontend.ApplicationService.Essential.Territory;
using WeeControl.Frontend.ApplicationService.Essential.User;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.Frontend.ApplicationService.Services;

namespace WeeControl.Frontend.ApplicationService;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<IPersistedLists, PersistedListService>();
        services.AddTransient<IServerOperation, ServerOperationService>();
        
        #region AdminContext
        services.AddTransient<TerritoryViewModel>();
        #endregion
        
        #region AuthorizationContext
        services.AddTransient<LoginViewModel>();
        services.AddTransient<LogoutViewModel>();
        services.AddTransient<UserViewModel>();
        services.AddTransient<PasswordResetViewModel>();
        services.AddTransient<PasswordChangeViewModel>();
        
        #endregion

        #region SharedContext
        services.AddTransient<SplashViewModel>();
        services.AddTransient<HomeViewModel>();
        services.AddTransient<HomeNavigationMenuViewModel>();
        #endregion
        
        return services;
    }
}