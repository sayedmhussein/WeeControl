using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using WeeControl.Frontend.AppService;

namespace WeeControl.Frontend.Wasm.Services;

public class DeviceDataService : IDeviceData
{
    private readonly IJSRuntime jsRuntime;
    private readonly NavigationManager navigationManager;
    private readonly IConfiguration configuration;
    public string ServerUrl => configuration["ApiBaseAddress"];
    public HttpClient HttpClient { get; init; }

    public DeviceDataService(IJSRuntime jsRuntime, NavigationManager navigationManager, IConfiguration configuration)
    {
        this.jsRuntime = jsRuntime;
        this.navigationManager = navigationManager;
        this.configuration = configuration;
        HttpClient = new HttpClient();
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

    public async Task<bool> IsConnectedToInternet()
    {
        return await jsRuntime.InvokeAsync<bool>("navigator.onLine");
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
        return Task.FromResult("__ThisIsBlaorApp");
    }

    public async Task<(double? Latitude, double? Longitude, double? Elevation)> GetDeviceLocation(bool accurate = false)
    {
        // var locationAvailable = await jsRuntime.InvokeAsync<bool>("position.coords.latitude");
        // if (locationAvailable)
        // {
        //     return (null, null, null);
        // }
        
        return (null, null, null);
    }

    public Task<bool> IsMockedDeviceLocation()
    {
        return Task.FromResult(true);
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