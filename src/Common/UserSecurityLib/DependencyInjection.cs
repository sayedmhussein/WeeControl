using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.UserSecurityLib.Helpers.CustomHandlers.TokenRefreshment;

namespace WeeControl.UserSecurityLib
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiAuthorizationService(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, TokenRefreshmentHandler>();
            services.AddAuthorization(UserAuthorizationOptions.Configure);
            
            return services;
        }
        
        public static IServiceCollection AddWasmAuthorizationService(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, TokenRefreshmentHandler>();
            services.AddAuthorizationCore(UserAuthorizationOptions.Configure);
            
            return services;
        }
    }
}