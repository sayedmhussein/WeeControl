using Microsoft.Extensions.DependencyInjection;
using WeeControl.Frontend.ApplicationService.Essential;
using WeeControl.Frontend.ApplicationService.Essential.Legacy;
using WeeControl.Frontend.ApplicationService.Essential.ViewModels;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.Frontend.ApplicationService.Services;

namespace WeeControl.Frontend.ApplicationService;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<IPersistedLists, PersistedListService>();
        services.AddTransient<IServerOperation, ServerOperationService>();

        services.AddTransient<HomeViewModel>();
        services.AddTransient<AuthorizationViewModel>();
        services.AddTransient<UserViewModel>();
        
        #region AdminContext
        services.AddTransient<TerritoryLegacyViewModel>();
        #endregion
        
        #region AuthorizationContext
        
        // services.AddTransient<LogoutLegacyViewModel>();
        services.AddTransient<UserLegacyViewModel>();
        services.AddTransient<PasswordResetLegacyViewModel>();
        services.AddTransient<PasswordChangeViewModel>();
        
        #endregion

        #region SharedContext
        // services.AddTransient<SplashLegacyViewModel>();
        // services.AddTransient<HomeLegacyViewModel>();
        // services.AddTransient<HomeNavigationMenuLegacyViewModel>();
        #endregion
        
        return services;
    }
}