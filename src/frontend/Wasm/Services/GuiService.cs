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
    private string currentPageName;

    public GuiService(IJSRuntime jsRuntime, NavigationManager navigationManager, IServiceProvider serviceProvider)
    {
        this.jsRuntime = jsRuntime;
        this.navigationManager = navigationManager;
        this.serviceProvider = serviceProvider;
    }

    string IGui.CurrentPageName => currentPageName;

    public Task DisplayAlert(string message, IGui.Severity severity = IGui.Severity.Normal)
    {
        jsRuntime.InvokeVoidAsync("alert", message);
        return Task.CompletedTask;
        // bool = jsRuntime.InvokeAsync<bool>("confirm", message);
        // string = await jsRuntime.InvokeAsync<string>("prompt", message);
    }
    
    public Task DisplayQuickAlert(string message, IGui.Severity severity = IGui.Severity.Normal)
    {
        using var scope = serviceProvider.CreateScope();
        var snackbar = scope.ServiceProvider.GetRequiredService<ISnackbar>();
        snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomStart;

        switch (severity)
        {
            case IGui.Severity.Normal:
                snackbar.Add(message);
                break;
            case IGui.Severity.Info:
                snackbar.Add(message, Severity.Info);
                break;
            case IGui.Severity.Success:
                snackbar.Add(message, Severity.Success);
                break;
            case IGui.Severity.Warning:
                snackbar.Add(message, Severity.Warning);
                break;
            case IGui.Severity.Error:
                snackbar.Add(message, Severity.Error);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
        }

        return Task.CompletedTask;
    }

    public Task NavigateToAsync(string pageName, bool forceLoad = false)
    {
        currentPageName = pageName;
        navigationManager.NavigateTo($"/{pageName}", forceLoad);
        return Task.CompletedTask;
    }
}