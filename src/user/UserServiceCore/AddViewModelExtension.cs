using Microsoft.Extensions.DependencyInjection;
using WeeControl.User.UserServiceCore.ViewModels.Authentication;
using WeeControl.User.UserServiceCore.ViewModels.Home;
using WeeControl.User.UserServiceCore.ViewModels.User;

namespace WeeControl.User.UserServiceCore;

public static class AddViewModelExtension
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        #region AuthorizationContext
        services.AddTransient<LoginViewModel>();
        services.AddTransient<LogoutViewModel>();
        services.AddTransient<RegisterViewModel>();
        services.AddTransient<ForgotMyPasswordViewModel>();
        services.AddTransient<SetNewPasswordViewModel>();
        
        #endregion

        #region SharedContext
        services.AddTransient<SplashViewModel>();
        services.AddTransient<HomeViewModel>();
        services.AddTransient<HomeNavigationMenuViewModel>();
        #endregion
        
        return services;
    }
}