using System.Security.Claims;

namespace WeeControl.Frontend.ApplicationService.Interfaces;

public interface IDeviceSecurity
{
    Task<bool> IsAuthenticatedAsync();

    Task UpdateTokenAsync(string token);

    Task<string> GetTokenAsync();

    Task DeleteTokenAsync();

    Task<IEnumerable<Claim>> GetClaimsAsync();
}