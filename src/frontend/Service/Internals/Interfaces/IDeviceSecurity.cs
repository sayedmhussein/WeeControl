using System.Runtime.CompilerServices;
using System.Security.Claims;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("ApplicationService.UnitTest")]
namespace WeeControl.Frontend.AppService.Internals.Interfaces;

internal interface IDeviceSecurity
{
    Task<bool> IsAuthenticatedAsync();

    Task UpdateTokenAsync(string token);

    Task<string?> GetTokenAsync();

    Task DeleteTokenAsync();

    Task<ClaimsPrincipal> GetClaimsPrincipal();

    event EventHandler<string> TokenChanged;
}