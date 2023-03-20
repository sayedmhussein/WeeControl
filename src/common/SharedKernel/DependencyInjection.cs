using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Core.SharedKernel.Services;

[assembly: InternalsVisibleTo("Core.Test")]
namespace WeeControl.Core.SharedKernel;

public static class DependencyInjection
{
    public static IServiceCollection AddUserSecurityService(this IServiceCollection services)
    {
        services.AddSingleton<IConstantValue, ConstantValueService>();
        services.AddSingleton<IJwtService, JwtService>();
        return services;
    }
}