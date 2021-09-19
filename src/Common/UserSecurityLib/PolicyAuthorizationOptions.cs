using Microsoft.AspNetCore.Authorization;
using WeeControl.UserSecurityLib.Policies;

namespace WeeControl.UserSecurityLib
{
    internal static class UserAuthorizationOptions
    {
        internal static void Configure(AuthorizationOptions options)
        {
            TerritoryOptions.Configure(options);
        }
    }
}