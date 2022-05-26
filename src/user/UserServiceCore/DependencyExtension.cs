using Microsoft.Extensions.DependencyInjection;
using WeeControl.SharedKernel.Essential;
using WeeControl.User.UserServiceCore.Interfaces;
using WeeControl.User.UserServiceCore.Services;
using WeeControl.User.UserServiceCore.ViewModels.Authentication;
using WeeControl.User.UserServiceCore.ViewModels.Home;

namespace WeeControl.User.UserServiceCore;

public static class DependencyExtension
{
    public static IServiceCollection AddUserServiceCore(this IServiceCollection services)
    {
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<IServerService, ServerService>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAdminService, AdminService>();

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