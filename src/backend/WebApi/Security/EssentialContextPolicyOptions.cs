using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.Services;
using WeeControl.WebApi.Security.CustomHandlers.TokenRefreshment;
using WeeControl.WebApi.Security.Policies;

namespace WeeControl.WebApi.Security;

internal static class EssentialContextPolicyOptions
{
    internal static IServiceCollection AddUserSecurityService(this IServiceCollection services)
    {
        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IPasswordSecurity, PasswordSecurity>();
        services.AddSingleton<IAuthorizationHandler, TokenRefreshmentHandler>();

        services.AddAuthorizationCore(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes("Bearer").RequireAuthenticatedUser().Build();
            options.FallbackPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes("Bearer").RequireAuthenticatedUser().Build();
                
            Configure(options);
        });

        return services;
    }
    
    private static void Configure(AuthorizationOptions options)
    {
        options.AddPolicy(DeveloperWithDatabaseOperationPolicy.Name, new DeveloperWithDatabaseOperationPolicy().GetPolicy());
        
        options.AddPolicy(nameof(CanEditUserPolicy), new CanEditUserPolicy().GetPolicy());
        options.AddPolicy(CanEditTerritoriesPolicy.Name, new CanEditTerritoriesPolicy().GetPolicy());
    }
}