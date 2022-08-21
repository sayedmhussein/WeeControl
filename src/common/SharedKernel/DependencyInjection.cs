using Microsoft.Extensions.DependencyInjection;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Services;

namespace WeeControl.Common.SharedKernel;

public static class DependencyInjection
{
    public static IServiceCollection AddUserSecurityService(this IServiceCollection services)
    {
        services.AddSingleton<IJwtService, JwtService>();
        return services;
    }
}