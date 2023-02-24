using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using WeeControl.Host.WebApiService;

namespace WeeControl.Frontend.Wasm.Services;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly ISecurity security;
    private readonly AuthenticationState anonymous;

    public AuthStateProvider(ISecurity security)
    {
        this.security = security;

        var identity = new ClaimsIdentity();
        var cp = new ClaimsPrincipal(identity);
        anonymous = new AuthenticationState(cp);

        security.AuthenticationChanged += ServiceDataOnAuthenticationChanged;
    }

    private async void ServiceDataOnAuthenticationChanged(object sender, bool e)
    {
        if (e)
        {
            NotifyUserAuthentication(await security.GetClaimsPrincipal());
            return;
        }

        NotifyUserAuthentication(null);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (await security.IsAuthenticated() == false)
        {
            return anonymous;
        }

        var cp = await security.GetClaimsPrincipal();
        return new AuthenticationState(cp);
    }

    private void NotifyUserAuthentication(ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal != null)
        {
            var state = new AuthenticationState(claimsPrincipal);
            var authState = Task.FromResult(state);
            NotifyAuthenticationStateChanged(authState);
        }
        else
        {
            NotifyAuthenticationStateChanged(Task.FromResult(anonymous));
        }
    }
}