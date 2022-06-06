using Microsoft.Extensions.DependencyInjection;
using WeeControl.User.UserApplication.ViewModels.Authentication;
using WeeControl.User.UserApplication.ViewModels.Essential;
using WeeControl.User.UserApplication.ViewModels.Home;
using WeeControl.User.UserApplication.ViewModels.User;

namespace WeeControl.User.UserApplication;

public static class ViewModelExtensions
{
    public static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        #region AdminContext
        services.AddTransient<ListOfUsersViewModel>();
        services.AddTransient<TerritoryViewModel>();

        #endregion
        
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