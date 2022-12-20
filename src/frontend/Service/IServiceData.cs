using System.Security.Claims;

namespace WeeControl.Frontend.AppService;

public interface IServiceData
{
    Task<bool> IsAuthenticated();
    Task<bool> IsConnectedToServer();
    Task<ClaimsPrincipal> GetClaimPrincipal();

    event EventHandler<bool> AuthenticationChanged;
}