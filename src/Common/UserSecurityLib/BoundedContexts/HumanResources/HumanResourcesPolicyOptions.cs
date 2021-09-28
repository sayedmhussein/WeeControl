using Microsoft.AspNetCore.Authorization;
using WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources.Policies;

namespace WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources
{
    internal static class HumanResourcesPolicyOptions
    {
        internal static void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(HumanResourcesData.Policies.CanAlterEmployee, new CanAlterEmployeePolicy().GetPolicy());
            options.AddPolicy(HumanResourcesData.Policies.CanAlterTerritories, new CanAlterTerritoriesPolicy().GetPolicy());
        }
    }
}