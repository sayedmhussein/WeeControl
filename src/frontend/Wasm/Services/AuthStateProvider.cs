using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;
using WeeControl.Frontend.AppService;

namespace WeeControl.Frontend.Wasm.Services;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly IServiceData serviceData;
    private readonly AuthenticationState anonymous;

    public AuthStateProvider(IServiceData serviceData)
    {
        this.serviceData = serviceData;

        var identity = new ClaimsIdentity();
        var cp = new ClaimsPrincipal(identity);
        anonymous = new AuthenticationState(cp);

        serviceData.AuthenticationChanged += ServiceDataOnAuthenticationChanged;
    }

    private async void ServiceDataOnAuthenticationChanged(object sender, bool e)
    {
        if (e)
        {
            NotifyUserAuthentication(await serviceData.GetClaimPrincipal());
            return;
        }

        NotifyUserAuthentication(null);
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (await serviceData.IsAuthenticated())
        {
            return anonymous;
        }

        var cp = await serviceData.GetClaimPrincipal();
        return new AuthenticationState(cp);
    }

    private void NotifyUserAuthentication(ClaimsPrincipal? claimsPrincipal)
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