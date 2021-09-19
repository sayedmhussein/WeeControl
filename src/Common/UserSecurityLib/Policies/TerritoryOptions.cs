using Microsoft.AspNetCore.Authorization;
using WeeControl.UserSecurityLib.Policies.Territory;

namespace WeeControl.UserSecurityLib.Policies
{
    public static class TerritoryOptions
    {
        public static void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(
                PolicyGroup.Territory.CanAlterTerritories, 
                new CanAlterTerritoriesPolicy().GetPolicy());
            
            
        }
    }
}