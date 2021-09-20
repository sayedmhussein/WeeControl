using Microsoft.AspNetCore.Authorization;
using WeeControl.Common.UserSecurityLib.EntityGroups.Territory;

namespace WeeControl.Common.UserSecurityLib.Helpers
{
    internal static class UserAuthorizationOptions
    {
        internal static void Configure(AuthorizationOptions options)
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes("Bearer").RequireAuthenticatedUser().Build();
            options.FallbackPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes("Bearer").RequireAuthenticatedUser().Build();
            
            TerritoryOptions.Configure(options);
        }
    }
}