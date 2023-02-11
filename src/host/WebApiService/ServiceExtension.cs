using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.SharedKernel;

namespace WeeControl.Host.WebApiService;

public static class ServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddUserSecurityService(); //From Shared Kernel

        return services;
    }
}