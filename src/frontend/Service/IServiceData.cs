using System.Security.Claims;

namespace WeeControl.Frontend.AppService;

public interface IServiceData
{
    Task<bool> IsAuthenticated();
    Task<ClaimsPrincipal> GetClaimPrincipal();
}