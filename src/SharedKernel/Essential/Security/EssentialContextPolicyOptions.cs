using Microsoft.AspNetCore.Authorization;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Essential.Security.Policies;

namespace WeeControl.SharedKernel.Essential.Security;

internal static class EssentialContextPolicyOptions
{
    internal static void Configure(AuthorizationOptions options)
    {
        options.AddPolicy(DeveloperWithDatabaseOperationPolicy.Name, new DeveloperWithDatabaseOperationPolicy().GetPolicy());
        
        options.AddPolicy(CanEditTerritoriesPolicy.Name, new CanEditUserPolicy().GetPolicy());
        options.AddPolicy(TerritoryDto.HttpPutMethod.CanEditTerritoryPolicy, new CanEditTerritoriesPolicy().GetPolicy());
    }
}