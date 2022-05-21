using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using WeeControl.Presentations.ServiceLibrary.Enums;
using WeeControl.Presentations.ServiceLibrary.Interfaces;

namespace WeeControl.Presentations.Wasm.Services;

public class DevicePageNavicationService : IDevicePageNavigation
{
    private readonly NavigationManager navigationManager;

    public DevicePageNavicationService(NavigationManager navigationManager)
    {
        this.navigationManager = navigationManager;
    }
    
    public Task NavigateToAsync(PagesEnum page, bool forceLoad = false)
    {
        navigationManager.NavigateTo("/" + Enum.GetName(typeof(PagesEnum), page), forceLoad);
        return Task.CompletedTask;
    }
}