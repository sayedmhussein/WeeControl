using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Application.Common.Behaviours;
using WeeControl.Common.SharedKernel.EntityGroups.Employee.Attributes;
using WeeControl.Common.SharedKernel.EntityGroups.Employee.Interfaces;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Attributes;
using WeeControl.Common.SharedKernel.EntityGroups.Territory.Interfaces;

namespace WeeControl.Backend.Application
{
    public static class DependancyExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestDtoBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            

            services.AddSingleton<ITerritoryAttribute, TerritoryAppSetting>();
            //
            services.AddSingleton<IEmployeeAttribute, EmployeeAttribute>();

            return services;
        }
    }
}
