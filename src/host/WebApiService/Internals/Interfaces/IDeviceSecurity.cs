using System.Security.Claims;

namespace WeeControl.Host.WebApiService.Internals.Interfaces;

internal interface IDeviceSecurity
{
    internal const string TokenKeyName = nameof(TokenKeyName);
    Task<bool> IsAuthenticated();

    Task UpdateToken(string token);

    Task<string?> GetToken();

    Task DeleteToken();

    Task<ClaimsPrincipal> GetClaimsPrincipal();

    event EventHandler<string> TokenChanged;
}