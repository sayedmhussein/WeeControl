using Microsoft.AspNetCore.Components.Authorization;
using WeeControl.Frontend.Wasm.Interfaces;

namespace WeeControl.Frontend.Wasm.Services;

public class SecurityService : ISecurityService
{
    private readonly AuthenticationStateProvider authProvider;

    public SecurityService(AuthenticationStateProvider authProvider)
    {
        this.authProvider = authProvider;
    }

    public void Update(string token)
    {
        ((AuthStateProvider)authProvider).NotifyUserAuthentication(token);
    }
}