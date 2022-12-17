using System.Threading.Tasks;
using Microsoft.JSInterop;
using WeeControl.Frontend.AppService.GuiInterfaces.Obsolute;

namespace WeeControl.Frontend.Wasm.Services;

public class DeviceAlertSimple : IDeviceAlert
{
    private readonly IJSRuntime jsRuntime;

    public DeviceAlertSimple(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task DisplayAlert(string alert)
    {
        await jsRuntime.InvokeVoidAsync("alert", alert);
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