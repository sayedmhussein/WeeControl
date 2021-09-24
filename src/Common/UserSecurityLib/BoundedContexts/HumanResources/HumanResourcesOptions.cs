using Microsoft.AspNetCore.Authorization;
using WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources.Policies;

namespace WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources
{
    public static class HumanResourcesOptions
    {
        public static void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(SecurityPolicies.Employee.CanAlterEmployee, new CanAlterEmployeePolicy().GetPolicy());
            options.AddPolicy(SecurityPolicies.Territory.CanAlterTerritories, new CanAlterTerritoriesPolicy().GetPolicy());
        }
    }
}