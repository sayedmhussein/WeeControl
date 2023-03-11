using System.Security.Claims;

namespace WeeControl.Host.WebApiService.Interfaces;

public interface ISecurity
{
    Task<ClaimsPrincipal> GetClaimsPrincipal();
    Task<bool> PageExistInClaims(string pageName, string? authority = null);
    Task<IEnumerable<string>> GetAllowedPages();
    Task<bool> IsAuthenticated();
    event EventHandler<bool> AuthenticationChanged;
}