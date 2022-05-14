using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources;
using WeeControl.Common.UserSecurityLib.Helpers.CustomHandlers.TokenRefreshment;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Common.UserSecurityLib.Services;

namespace WeeControl.Common.UserSecurityLib;

public static class DependencyInjection
{
    public static IServiceCollection AddUserSecurityService(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordSecurity, PasswordSecurity>();
        
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