using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.ApiApp.Application.Behaviours;

namespace WeeControl.ApiApp.Application;

public static class DependencyExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        return services;
    }
}