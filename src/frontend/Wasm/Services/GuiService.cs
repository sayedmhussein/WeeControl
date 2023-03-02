using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using MudBlazor;
using WeeControl.Host.WebApiService.DeviceInterfaces;

namespace WeeControl.Frontend.Wasm.Services;

public class GuiService : IGui
{
    private readonly IJSRuntime jsRuntime;
    private readonly NavigationManager navigationManager;
    private readonly IServiceProvider serviceProvider;
    private string lastPageName;
    private string currentPageName;

    public GuiService(IJSRuntime jsRuntime, NavigationManager navigationManager, IServiceProvider serviceProvider)
    {
        this.jsRuntime = jsRuntime;
        this.navigationManager = navigationManager;
        this.serviceProvider = serviceProvider;
    }

    string IGui.CurrentPageName => currentPageName;

    public Task DisplayAlert(string message)
    {
        jsRuntime.InvokeVoidAsync("alert", message);
        return Task.CompletedTask;
        // bool = jsRuntime.InvokeAsync<bool>("confirm", message);
        // string = await jsRuntime.InvokeAsync<string>("prompt", message);
    }

    public Task DisplayQuickAlert(string message)
    {
        using var scope = serviceProvider.CreateScope();
        var snackbar = scope.ServiceProvider.GetRequiredService<ISnackbar>();
        snackbar.Add(message);
        return Task.CompletedTask;
    }

    public Task NavigateToAsync(string pageName, bool forceLoad = false)
    {
        lastPageName = pageName;
        navigationManager.NavigateTo($"/{pageName}", forceLoad: forceLoad);
        return Task.CompletedTask;
    }
}