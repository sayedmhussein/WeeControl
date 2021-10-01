using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.SharedKernel.Common.Interfaces;
using WeeControl.SharedKernel.Security.BoundedContexts.HumanResources;
using WeeControl.SharedKernel.Security.Helpers.CustomHandlers.TokenRefreshment;
using WeeControl.SharedKernel.Security.Services;

namespace WeeControl.SharedKernel.Security
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUserSecurityService(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, TokenRefreshmentHandler>();

            services.AddSingleton<IJwtService, JwtService>();

            services.AddAuthorizationCore(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes("Bearer").RequireAuthenticatedUser().Build();
                options.FallbackPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes("Bearer").RequireAuthenticatedUser().Build();
                
                HumanResourcesPolicyOptions.Configure(options);
            });

            return services;
        }
    }
}