using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Core.SharedKernel.Services;

namespace WeeControl.Core.SharedKernel;

public static class DependencyInjection
{
    public static IServiceCollection AddUserSecurityService(this IServiceCollection services)
    {
        services.AddSingleton<IJwtService, JwtService>();
        return services;
    }
}