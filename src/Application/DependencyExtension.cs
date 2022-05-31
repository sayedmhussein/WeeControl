using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace WeeControl.Application;

public static class DependencyExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestDtoBehaviour<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
        return services;
    }
}