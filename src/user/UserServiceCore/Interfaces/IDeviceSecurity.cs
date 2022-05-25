using System.Security.Claims;

namespace WeeControl.User.UserServiceCore.Interfaces;

public interface IDeviceSecurity
{
    Task<bool> IsAuthenticatedAsync();

    Task UpdateTokenAsync(string token);

    Task DeleteTokenAsync();

    Task<IEnumerable<Claim>> GetClaimsAsync();
}