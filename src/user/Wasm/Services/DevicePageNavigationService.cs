using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.Wasm.Services;

public class DevicePageNavigationService : IDevicePageNavigation
{
    private readonly NavigationManager navigationManager;

    public DevicePageNavigationService(NavigationManager navigationManager)
    {
        this.navigationManager = navigationManager;
    }

    public Task NavigateToAsync(PagesEnum page)
    {
        navigationManager.NavigateTo("/" + Enum.GetName(typeof(PagesEnum), page));
        return Task.CompletedTask;
    }

    public Task NavigateToAsync(PagesEnum page, bool forceLoad)
    {
        navigationManager.NavigateTo("/" + Enum.GetName(typeof(PagesEnum), page), forceLoad);
        return Task.CompletedTask;
    }

    public Task NavigateToAsync(PagesEnum page, bool forceLoad = false, bool disableBackButton = false)
    {
        navigationManager.NavigateTo("/" + Enum.GetName(typeof(PagesEnum), page), forceLoad);
        return Task.CompletedTask;
    }
}