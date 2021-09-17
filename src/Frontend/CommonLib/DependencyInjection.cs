using System;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Frontend.CommonLib.DataAccess;
using WeeControl.Frontend.CommonLib.DataAccess.Employee;
using WeeControl.Frontend.CommonLib.Interfaces;
using WeeControl.Frontend.CommonLib.Services;
using WeeControl.SharedKernel.Routing;

namespace WeeControl.Frontend.CommonLib
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommonLibraryService(this IServiceCollection services)
        {
            services.AddSingleton<IApiRoute, ApiRoute>();
            services.AddTransient<IHttpService, HttpService>();
            
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            
            services.AddSingleton<IEmployeeData, EmployeeData>();

            return services;
        }
        
    }

}
