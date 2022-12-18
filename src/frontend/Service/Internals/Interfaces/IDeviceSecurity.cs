using System.Security.Claims;

namespace WeeControl.Frontend.AppService.Internals.Interfaces;

internal interface IDeviceSecurity
{
    Task<bool> IsAuthenticatedAsync();

    Task UpdateTokenAsync(string token);

    Task<string?> GetTokenAsync();

    Task DeleteTokenAsync();

    Task<ClaimsPrincipal> GetClaimsPrincipal();
}