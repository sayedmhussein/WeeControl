using System.Security.Claims;
using WeeControl.Core.SharedKernel;
using WeeControl.Host.WebApiService.Data;
using WeeControl.Host.WebApiService.DeviceInterfaces;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.WebApiService.Internals.Services;

internal class AppSecurityService : ISecurity
{
    private readonly IGui gui;
    private readonly IDeviceSecurity security;

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

    public async Task<bool> PageExistInClaims(string pageName, string? authority)
    {
        if (ClaimsValues.GetClaimTypes().TryGetValue(pageName, out var val))
        {
            var foundClaim = (await security.GetClaimsPrincipal()).Claims.Where(x => x.Type == val);
            if (foundClaim.Any())
            {
                if (string.IsNullOrEmpty(authority))
                    return true;

                if (foundClaim.FirstOrDefault(x => x.Value == authority) != null) return true;
            }
        }

        return false;
    }

    public async Task<IEnumerable<string>> GetAllowedPages()
    {
        var list = new List<string>();
        foreach (var p in ApplicationPages.Elevator.GetListOfPages())
            if (await PageExistInClaims(p.Value, null))
                list.Add(p.Value);

        return list;
    }

    public Task<bool> IsAuthenticated()
    {
        return security.IsAuthenticated();
    }

    public event EventHandler<bool>? AuthenticationChanged;

    public async Task NavigateToNecessaryPage()
    {
        if (await IsAuthenticated())
        {
            await gui.NavigateToAsync(ApplicationPages.Essential.HomePage, true);
            return;
        }

        if (gui.CurrentPageName != ApplicationPages.Essential.LoginPage)
            await gui.NavigateToAsync(ApplicationPages.Essential.LoginPage, true);
    }
}