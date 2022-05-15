using Microsoft.AspNetCore.Authorization;
using WeeControl.SharedKernel.Essential.Policies;

namespace WeeControl.SharedKernel.Essential;

internal static class HumanResourcesPolicyOptions
{
    internal static void Configure(AuthorizationOptions options)
    {
        options.AddPolicy(HumanResourcesData.Policies.CanAlterEmployee, new CanAlterEmployeePolicy().GetPolicy());
        options.AddPolicy(HumanResourcesData.Policies.CanAlterTerritories, new CanAlterTerritoriesPolicy().GetPolicy());
    }
}