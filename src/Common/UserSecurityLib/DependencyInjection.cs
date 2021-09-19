using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.UserSecurityLib.Helpers;
using WeeControl.UserSecurityLib.Helpers.CustomHandlers.TokenRefreshment;
using WeeControl.UserSecurityLib.Interfaces;
using WeeControl.UserSecurityLib.Services;

namespace WeeControl.UserSecurityLib
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiAuthorizationService(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, TokenRefreshmentHandler>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddAuthorizationCore(UserAuthorizationOptions.Configure);
            //services.AddAuthorization(UserAuthorizationOptions.Configure);
            
            return services;
        }
        
        public static IServiceCollection AddWasmAuthorizationService(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, TokenRefreshmentHandler>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddAuthorizationCore(UserAuthorizationOptions.Configure);
            
            return services;
        }
    }
}