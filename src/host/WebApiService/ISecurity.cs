using System.Security.Claims;

namespace WeeControl.Host.WebApiService;

public interface ISecurity
{
     Task<ClaimsPrincipal> GetClaimsPrincipal();
     Task<bool> IsAuthenticated();
     event EventHandler<bool> AuthenticationChanged;
}