using System.Security.Claims;
using WeeControl.Frontend.AppService.Interfaces;

namespace WeeControl.Frontend.AppService.Services;

internal class SecurityService : IDeviceSecurity
{
    public Task<bool> IsAuthenticatedAsync()
    {
        throw new NotImplementedException();
    }

    public Task UpdateTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetTokenAsync()
    {
        throw new NotImplementedException();
    }

    public Task DeleteTokenAsync()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Claim>> GetClaimsAsync()
    {
        throw new NotImplementedException();
    }
}