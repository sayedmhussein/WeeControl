using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using WeeControl.Host.WebApiService.DeviceInterfaces;

namespace WeeControl.Frontend.Wasm.Services;

public class DeviceDataService : ICommunication, IFeature, IMedia, ISharing, IStorage
{
    private readonly IConfiguration configuration;
    private readonly IJSRuntime jsRuntime;
    private readonly NavigationManager navigationManager;
    private readonly IServiceProvider serviceProvider;

    public DeviceDataService(IJSRuntime jsRuntime, NavigationManager navigationManager, IConfiguration configuration,
        IServiceProvider serviceProvider)
    {
        this.jsRuntime = jsRuntime;
        this.navigationManager = navigationManager;
        this.configuration = configuration;
        this.serviceProvider = serviceProvider;
        HttpClient = new HttpClient();
    }


    public string ServerUrl => configuration["ApiBaseAddress"];
    public HttpClient HttpClient { get; init; }

    public async Task SendAnEmail(IEnumerable<string> to, string subject, string body)
    {
        await jsRuntime.InvokeAsync<bool>($"Sending Email to {to}", body);
    }

    public async Task SendAnEmail(IEnumerable<string> to, string subject, string body, IEnumerable<string> attachments)
    {
        await jsRuntime.InvokeAsync<bool>($"Sending Email to {to}", body);
    }

    public async Task SendSms(IEnumerable<string> to, string text)
    {
        await jsRuntime.InvokeAsync<bool>($"Sending SMS to {to}", text);
    }

    public Task<bool> IsConnectedToInternet()
    {
        return Task.FromResult(true);
        //return await jsRuntime.InvokeAsync<bool>("window.navigator.onLine");
    }

    public async Task<bool> PhoneDial(string phoneNo)
    {
        await jsRuntime.InvokeAsync<bool>("Phone Call", $"Please call the following number from your mobile {phoneNo}");
        return true;
    }

    public Task<bool> IsEnergySavingMode()
    {
        return Task.FromResult(false);
    }

    public Task<string> GetDeviceId()
    {
        return Task.FromResult("__ThisIsBlazorApp");
    }

    public async Task<(double? Latitude, double? Longitude, double? Elevation)> GetDeviceLocation(bool accurate = false)
    {
        await Task.Delay(10);
        return (null, null, null);
    }

    public Task<bool> IsMockedDeviceLocation()
    {
        return Task.FromResult(true);
    }

    public Task Speak(string message)
    {
        throw new NotImplementedException();
    }

    public Task CopyToClipboard(string text)
    {
        throw new NotImplementedException();
    }

    public Task<string> ReadFromClipboard()
    {
        throw new NotImplementedException();
    }

    public Task ClearClipboard()
    {
        return Task.CompletedTask;
    }

    public string CashDirectory { get; }
    public string AppDataDirectory { get; }

    public Task SaveKeyValue(string key, string value)
    {
        jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value)).ConfigureAwait(false);
        return Task.CompletedTask;
    }

    public async Task<string> GetKeyValue(string key)
    {
        var json = await jsRuntime.InvokeAsync<string>("localStorage.getItem", key).ConfigureAwait(false);
        return json == null ? default : JsonSerializer.Deserialize<string>(json);
    }

    public Task ClearKeysValues()
    {
        jsRuntime.InvokeVoidAsync("localStorage.clear").ConfigureAwait(false);
        return Task.CompletedTask;
    }
}