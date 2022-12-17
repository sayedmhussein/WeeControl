using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using WeeControl.Frontend.AppService;

namespace WeeControl.Frontend.Wasm.Services;

public class DeviceDataService : IDeviceData
{
    private readonly IJSRuntime jsRuntime;
    private readonly NavigationManager navigationManager;
    public string ServerUrl { get; }
    public HttpClient HttpClient { get; set; }

    public DeviceDataService(IJSRuntime jsRuntime, NavigationManager navigationManager)
    {
        this.jsRuntime = jsRuntime;
        this.navigationManager = navigationManager;
    }
    
    public Task SendAnEmail(IEnumerable<string> to, string subject, string body)
    {
        throw new System.NotImplementedException();
    }

    public Task SendAnEmail(IEnumerable<string> to, string subject, string body, IEnumerable<string> attachments)
    {
        throw new System.NotImplementedException();
    }

    public Task SendSms(IEnumerable<string> to, string text)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> IsConnectedToInternet()
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> PhoneDial(string phoneNo)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> IsEnergySavingMode()
    {
        throw new System.NotImplementedException();
    }

    public Task<string> GetDeviceId()
    {
        throw new System.NotImplementedException();
    }

    public Task<(double? Latitude, double? Longitude, double? Elevation)> GetDeviceLocation(bool accurate = false)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> IsMockedDeviceLocation()
    {
        throw new System.NotImplementedException();
    }

    public Task DisplayAlert(string message)
    {
        jsRuntime.InvokeVoidAsync("alert", message);
        return Task.CompletedTask;
        // bool = jsRuntime.InvokeAsync<bool>("confirm", message);
        // string = await jsRuntime.InvokeAsync<string>("prompt", message);
    }

    public Task NavigateToAsync(string pageName, bool forceLoad = false)
    {
        navigationManager.NavigateTo($"/{pageName}", forceLoad: forceLoad);
        return Task.CompletedTask;
    }

    public Task Speak(string message)
    {
        throw new System.NotImplementedException();
    }

    public Task CopyToClipboard(string text)
    {
        throw new System.NotImplementedException();
    }

    public Task<string> ReadFromClipboard()
    {
        throw new System.NotImplementedException();
    }

    public Task ClearClipboard()
    {
        throw new System.NotImplementedException();
    }

    public string CashDirectory { get; }
    public string AppDataDirectory { get; }
    public Task SaveAsync(string key, string value)
    {
        jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value)).ConfigureAwait(false);
        return Task.CompletedTask;
    }

    public async Task<string> GetAsync(string key)
    {
        var json = await jsRuntime.InvokeAsync<string>("localStorage.getItem", key).ConfigureAwait(false);
        return json == null ? default : JsonSerializer.Deserialize<string>(json);
    }

    public Task ClearAsync()
    {
        jsRuntime.InvokeVoidAsync("localStorage.clear").ConfigureAwait(false);
        return Task.CompletedTask;
    }
}