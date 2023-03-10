using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Application.Services;
using WeeControl.Core.SharedKernel.Interfaces;
using WeeControl.Core.SharedKernel.Services;
using WeeControl.Host.WebApi.Services.Security.CustomHandlers.TokenRefreshment;
using WeeControl.Host.WebApi.Services.Security.Policies;

namespace WeeControl.Host.WebApi.Services.Security;

internal static class UserSecurityServices
{
    internal static IServiceCollection AddUserSecurityService(this IServiceCollection services)
    {
        services.AddSingleton<IJwtService, JwtService>();
        services.AddSingleton<IPasswordSecurity, PasswordSecurity>();
        services.AddSingleton<IAuthorizationHandler, TokenRefreshmentHandler>();

        services.AddAuthorizationCore(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes("Bearer")
                .RequireAuthenticatedUser().Build();
            options.FallbackPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes("Bearer")
                .RequireAuthenticatedUser().Build();

            Configure(options);
        });

        return services;
    }

    private static void Configure(AuthorizationOptions options)
    {
        options.AddPolicy(DeveloperWithDatabaseOperationPolicy.Name,
            new DeveloperWithDatabaseOperationPolicy().GetPolicy());

        options.AddPolicy(nameof(CanEditUserPolicy), new CanEditUserPolicy().GetPolicy());
        options.AddPolicy(CanEditTerritoriesPolicy.Name, new CanEditTerritoriesPolicy().GetPolicy());
    }
}