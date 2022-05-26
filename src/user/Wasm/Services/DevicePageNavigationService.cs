using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Interfaces;

namespace WeeControl.User.Wasm.Services;

public class DevicePageNavigationService : IDevicePageNavigation
{
    private readonly NavigationManager navigationManager;
    private readonly ICollection<PagesEnum> history;

    public DevicePageNavigationService(NavigationManager navigationManager)
    {
        this.navigationManager = navigationManager;
        
        history = new List<PagesEnum>();
        history.Add(PagesEnum.Splash);
    }

    public Task NavigateToAsync(string pageName, bool forceLoad = false)
    {
        navigationManager.NavigateTo($"/{pageName}", forceLoad: forceLoad);
        return Task.CompletedTask;
    }

    public Task NavigateToAsync(PagesEnum page)
    {
        navigationManager.NavigateTo("/" + GetPageName(page));
        return Task.CompletedTask;
    }

    public Task NavigateToAsync(PagesEnum page, bool forceLoad)
    {
        history.Add(page);

        navigationManager.NavigateTo("/" + GetPageName(page), forceLoad);
        return Task.CompletedTask;
    }

    public Task GoBackAsync()
    {
        return NavigateToAsync(history.LastOrDefault());
    }
    
    private void AddToHistory(PagesEnum page)
    {
        history.Add(page);
    }

    private string GetPageName(PagesEnum page)
    {
        return Enum.GetName(typeof(PagesEnum), page);
    }
}