using Microsoft.AspNetCore.Authorization;
using WeeControl.SharedKernel.Essential.RequestDTOs;
using WeeControl.SharedKernel.Essential.Security.Policies;

namespace WeeControl.SharedKernel.Essential.Security;

internal static class EssentialContextPolicyOptions
{
    internal static void Configure(AuthorizationOptions options)
    {
        options.AddPolicy(UserDto.HttpPutMethod.CanEditUserPolicy, new CanEditUserPolicy().GetPolicy());
        options.AddPolicy(TerritoryDto.HttpPutMethod.CanEditTerritoryPolicy, new CanEditTerritoriesPolicy().GetPolicy());
    }
}