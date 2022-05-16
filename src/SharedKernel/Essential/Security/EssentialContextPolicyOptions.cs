using Microsoft.AspNetCore.Authorization;
using WeeControl.SharedKernel.Essential.Security.Policies;

namespace WeeControl.SharedKernel.Essential.Security;

internal static class EssentialContextPolicyOptions
{
    internal static void Configure(AuthorizationOptions options)
    {
        options.AddPolicy(ClaimsTagsList.Policies.CanAlterEmployee, new CanAlterEmployeePolicy().GetPolicy());
        options.AddPolicy(ClaimsTagsList.Policies.CanAlterTerritories, new CanAlterTerritoriesPolicy().GetPolicy());
    }
}