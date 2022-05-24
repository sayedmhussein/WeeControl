using Microsoft.Extensions.DependencyInjection;
using WeeControl.SharedKernel.Essential;
using WeeControl.User.UserServiceCore.InternalHelpers;
using WeeControl.User.UserServiceCore.Services;

namespace WeeControl.User.UserServiceCore;

public static class DependencyExtension
{
    public static IServiceCollection AddUserServiceCore(this IServiceCollection services)
    {
        services.AddScoped<IServerService, ServerService>();
        services.AddScoped<IAdminService, AdminService>();
        
        return services;
    }
}