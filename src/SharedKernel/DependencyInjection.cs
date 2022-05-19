using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.SharedKernel.Essential.Security;
using WeeControl.SharedKernel.Essential.Security.CustomHandlers.TokenRefreshment;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.Services;

namespace WeeControl.SharedKernel;

public static class DependencyInjection
{
    public static IServiceCollection AddUserSecurityServiceForServer(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordSecurity, PasswordSecurity>();
        
        services.AddSingleton<IAuthorizationHandler, TokenRefreshmentHandler>();

        services.AddSingleton<IJwtService, JwtService>();

        services.AddAuthorizationCore(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes("Bearer").RequireAuthenticatedUser().Build();
            options.FallbackPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes("Bearer").RequireAuthenticatedUser().Build();
                
            EssentialContextPolicyOptions.Configure(options);
        });

        return services;
    }

    public static IServiceCollection AddUserSecurityServiceForApplication(this IServiceCollection services)
    {
        services.AddSingleton<IJwtService, JwtService>();
        return services;
    }
}