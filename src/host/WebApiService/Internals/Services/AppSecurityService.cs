using System.Security.Claims;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Internals.Services;

internal class AppSecurityService : ISecurity
{
    private readonly IDeviceSecurity security;
    private readonly IGui gui;

    public AppSecurityService(IDeviceSecurity security, IGui gui)
    {
        this.security = security;
        this.gui = gui;
        security.TokenChanged += (sender, s) =>
        AuthenticationChanged?.Invoke(this, !string.IsNullOrEmpty(s));
    }
    
    public Task<ClaimsPrincipal> GetClaimsPrincipal()
    {
        return security.GetClaimsPrincipal();
    }

    public Task<bool> IsAuthenticated()
    {
        return security.IsAuthenticated();
    }

    public async Task NavigateToNecessaryPage()
    {
        if (await IsAuthenticated())
        {
            await gui.NavigateToAsync(ApplicationPages.Essential.HomePage, forceLoad: true);
            return;
        }

        await gui.NavigateToAsync(ApplicationPages.Essential.LoginPage, forceLoad: true);
    }

    public event EventHandler<bool>? AuthenticationChanged;
}