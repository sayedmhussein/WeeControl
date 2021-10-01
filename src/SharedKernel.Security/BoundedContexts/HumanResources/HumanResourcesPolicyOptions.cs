using Microsoft.AspNetCore.Authorization;
using WeeControl.SharedKernel.Security.BoundedContexts.HumanResources.Policies;

namespace WeeControl.SharedKernel.Security.BoundedContexts.HumanResources
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