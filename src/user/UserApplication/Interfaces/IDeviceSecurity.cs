using System.Security.Claims;

namespace WeeControl.User.UserApplication.Interfaces;

public interface IDeviceSecurity
{
    Task<bool> IsAuthenticatedAsync();

    Task UpdateTokenAsync(string token);

    Task<string> GetTokenAsync();

    Task DeleteTokenAsync();

    Task<IEnumerable<Claim>> GetClaimsAsync();
    
    string Token { get; }
}