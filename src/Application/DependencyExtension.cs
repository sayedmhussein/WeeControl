using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Application.Behaviours;

namespace WeeControl.Application;

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