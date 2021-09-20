using Microsoft.AspNetCore.Authorization;
using WeeControl.Common.UserSecurityLib.EntityGroups.Territory.Policies;

namespace WeeControl.Common.UserSecurityLib.EntityGroups.Territory
{
    public static class TerritoryOptions
    {
        public static void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(
                CustomAuthorizationPolicy.Territory.CanAlterTerritories, 
                new CanAlterTerritoriesPolicy().GetPolicy());
        }
    }
}