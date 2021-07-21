﻿using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.SharedKernel.BasicSchemas.Employee;
using WeeControl.SharedKernel.BasicSchemas.Territory;

namespace WeeControl.Server.Application
{
    public static class DependancyExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            

            services.AddSingleton<ITerritoryLists, TerritoryLists>();
            //
            services.AddSingleton<IEmployeeLists, EmployeeLists>();

            return services;
        }
    }
}