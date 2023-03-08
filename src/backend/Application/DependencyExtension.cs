using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WeeControl.Core.Application.Behaviours;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Application.Services;

namespace WeeControl.Core.Application;

public static class DependencyExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));

        services.AddSingleton<IPasswordSecurity, PasswordSecurity>();
        return services;
    }
}