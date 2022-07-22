using Microsoft.Extensions.DependencyInjection;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.Services;

namespace WeeControl.SharedKernel;

public static class DependencyInjection
{
    public static IServiceCollection AddUserSecurityService(this IServiceCollection services)
    {
        services.AddSingleton<IJwtService, JwtService>();
        return services;
    }
}