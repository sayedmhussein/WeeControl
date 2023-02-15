using System.Security.Claims;

namespace WeeControl.Host.WebApiService.Internals.Interfaces;

internal interface IDeviceSecurity
{
    Task<bool> IsAuthenticatedAsync();

    Task UpdateTokenAsync(string token);

    Task<string?> GetTokenAsync();

    Task DeleteTokenAsync();

    Task<ClaimsPrincipal> GetClaimsPrincipal();

    event EventHandler<string> TokenChanged;
}