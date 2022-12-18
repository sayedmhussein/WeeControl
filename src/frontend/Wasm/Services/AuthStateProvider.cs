using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WeeControl.Common.SharedKernel.Contexts.Authentication;
using WeeControl.Common.SharedKernel.Interfaces;
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

    private void NotifyUserAuthentication(string token)
    {
        if (string.IsNullOrWhiteSpace(token) == false)
        {
            var cp = serviceData.GetClaimPrincipal();
            var state = new AuthenticationState(cp.GetAwaiter().GetResult());
            var authState = Task.FromResult(state);
            NotifyAuthenticationStateChanged(authState);
        }
        else
        {
            NotifyAuthenticationStateChanged(Task.FromResult(anonymous));
        }
    }
}