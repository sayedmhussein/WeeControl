using Microsoft.AspNetCore.Authorization;
using WeeControl.UserSecurityLib.EntityGroups.Territory;

namespace WeeControl.UserSecurityLib.Helpers
{
    internal static class UserAuthorizationOptions
    {
        internal static void Configure(AuthorizationOptions options)
        {
            TerritoryOptions.Configure(options);
        }
    }
}