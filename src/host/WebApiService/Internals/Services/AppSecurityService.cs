using System.Security.Claims;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Internals.Services;

internal class AppSecurityService : ISecurity
{
    private readonly IDeviceSecurity security;

    public AppSecurityService(IDeviceSecurity security)
    {
        this.security = security;
    }
    
    public Task<ClaimsPrincipal> GetClaimsPrincipal()
    {
        return security.GetClaimsPrincipal();
    }

    public Task<bool> IsAuthenticated()
    {
        return security.IsAuthenticated();
    }

    public event EventHandler<bool>? AuthenticationChanged;
}