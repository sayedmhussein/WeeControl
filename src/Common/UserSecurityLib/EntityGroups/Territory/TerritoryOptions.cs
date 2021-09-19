using Microsoft.AspNetCore.Authorization;
using WeeControl.UserSecurityLib.EntityGroups.Territory.Policies;

namespace WeeControl.UserSecurityLib.EntityGroups.Territory
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