using Microsoft.Extensions.DependencyInjection;
using WeeControl.Frontend.CommonLib.DataAccess.Authentication;
using WeeControl.Frontend.CommonLib.DataAccess.Employee;
using WeeControl.Frontend.CommonLib.Interfaces;
using WeeControl.Frontend.CommonLib.Services;

namespace WeeControl.Frontend.CommonLib
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommonLibraryService(this IServiceCollection services)
        {
            services.AddTransient<IHttpService, HttpService>();

            
            //services.AddScoped<IJwtService>(x => new JwtService("This is JWT Key, Please keep it for as secured and never share it with any one under any reason!"));
            
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            
            services.AddSingleton<IEmployeeData, EmployeeData>();

            return services;
        }
    }

}
