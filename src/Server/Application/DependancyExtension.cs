using System;
using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Server.Application.Common.Interfaces;
using WeeControl.SharedKernel.BasicSchemas.Employee.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Territory.Dicts;
using WeeControl.SharedKernel.BasicSchemas.Territory.Interfaces;

namespace WeeControl.Server.Application
{
    public static class DependancyExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            

            services.AddSingleton<ITerritoryDicts, TerritoryDicts>();
            //
            services.AddSingleton<IClaimDicts, ClaimDicts>();
            services.AddSingleton<IIdentityDicts, IdentityDicts>();
            services.AddSingleton<IPersonalAttribDicts, PersonalAttribDicts>();

            return services;
        }
    }
}
