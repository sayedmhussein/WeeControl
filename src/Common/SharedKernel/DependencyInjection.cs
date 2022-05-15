using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Common.SharedKernel.Essential;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Common.SharedKernel.Services;
using WeeControl.Common.SharedKernel.Services.PolicyBuild.CustomHandlers.TokenRefreshment;

namespace WeeControl.Common.SharedKernel;

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