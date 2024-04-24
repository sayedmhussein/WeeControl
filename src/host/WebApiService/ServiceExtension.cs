using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.SharedKernel;
using WeeControl.Host.WebApiService.Contexts.Essentials;
using WeeControl.Host.WebApiService.Contexts.Essentials.Services;
using WeeControl.Host.WebApiService.Interfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;
using WeeControl.Host.WebApiService.Internals.Services;

[assembly: InternalsVisibleTo("Host.Test")]

namespace WeeControl.Host.WebApiService;

public static class ServiceExtension
{
    public static IServiceCollection AddWebApiService(this IServiceCollection services)
    {
        services.AddUserSecurityService(); //From Shared Kernel

        services.AddContexts();
        services.AddInternals();

        return services;
    }

    private static IServiceCollection AddContexts(this IServiceCollection services)
    {
        services.AddSingleton<IPersonService, PersonService>();
        services.AddSingleton<IAuthenticationService, AuthenticationService>();
        services.AddSingleton<IHomeService, HomeService>();

        return services;
    }

    private static IServiceCollection AddInternals(this IServiceCollection services)
    {
        services.AddSingleton<IConstantValue, ConstantValueService>();
        services.AddSingleton<IDatabaseService, DatabaseService>();
        services.AddSingleton<IDeviceSecurity, SecurityService>();
        services.AddSingleton<IServerOperation, ServerService>();
        services.AddSingleton<ISecurity, AppSecurityService>();

        return services;
    }
}