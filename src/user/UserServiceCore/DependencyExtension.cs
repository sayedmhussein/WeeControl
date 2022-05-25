using Microsoft.Extensions.DependencyInjection;
using WeeControl.SharedKernel.Essential;
using WeeControl.User.UserServiceCore.Interfaces;
using WeeControl.User.UserServiceCore.InternalHelpers;
using WeeControl.User.UserServiceCore.Services;
using WeeControl.User.UserServiceCore.ViewModels.Essentials;
using WeeControl.User.UserServiceCore.ViewModels.Shared;

namespace WeeControl.User.UserServiceCore;

public static class DependencyExtension
{
    public static IServiceCollection AddUserServiceCore(this IServiceCollection services)
    {
        services.AddScoped<IServerService, ServerService>();
        
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAdminService, AdminService>();

        #region EssentialContext
        services.AddTransient<LoginViewModel>();
        services.AddTransient<LogoutViewModel>();
        services.AddTransient<RegisterViewModel>();
        services.AddTransient<ForgotMyPasswordViewModel>();
        services.AddTransient<SetNewPasswordViewModel>();
        #endregion

        #region SharedContext

        services.AddTransient<HomeViewModel>();

        #endregion
        
        
        return services;
    }
}