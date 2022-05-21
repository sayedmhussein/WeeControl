using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using WeeControl.Presentations.ServiceLibrary.Enums;
using WeeControl.Presentations.ServiceLibrary.Interfaces;

namespace WeeControl.Presentations.Wasm.Services;

public class DeviceAlertSimple : IDeviceAlert
{
    private readonly IJSRuntime jsRuntime;

    public DeviceAlertSimple(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task DisplayAlert(AlertEnum alertEnum)
    {
        await jsRuntime.InvokeVoidAsync("alert", Enum.GetName(typeof(AlertEnum), alertEnum));
    }

    public async Task DisplaySimpleAlertAsync(string message)
    {
        await jsRuntime.InvokeVoidAsync("alert", message);
    }

    public async Task<bool> DisplayBooleanAlertAsync(string message)
    {
        return await jsRuntime.InvokeAsync<bool>("confirm", message);
    }

    public async Task<string> DisplayPromptedAlertAsync(string message)
    {
        return await jsRuntime.InvokeAsync<string>("prompt", message);
    }
}