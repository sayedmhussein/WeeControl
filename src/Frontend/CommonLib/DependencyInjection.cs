using System;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Frontend.CommonLib.DataAccess;
using WeeControl.Frontend.CommonLib.DataAccess.Employee;
using WeeControl.SharedKernel.Routing;

namespace WeeControl.Frontend.CommonLib
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRefitService(this IServiceCollection services)
        {
            services.AddSingleton<IEmployeeData, EmployeeData>();
            //services.AddSingleton<IApiRoute, ApiRoute>();
            
            //services.AddRefitClient<ITerritoryData>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:5001/"));
            //services.AddRefitClient<IEmployeeData>().ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:5001/"));
            
            return services;
        }
        
    }

}
