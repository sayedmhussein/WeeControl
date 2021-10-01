using Microsoft.Extensions.DependencyInjection;
using WeeControl.App.Services.Authorization;

namespace WeeControl.App.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserService(this IServiceCollection services)
        {
            #region Adminstration
            #endregion

            #region Authorization

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            #endregion
            
            return services;
        }
    }
}