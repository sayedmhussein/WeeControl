using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Application.Behaviours;

namespace WeeControl.Backend.Application
{
    public static class DependancyExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestDtoBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));
            

            return services;
        }
    }
}
