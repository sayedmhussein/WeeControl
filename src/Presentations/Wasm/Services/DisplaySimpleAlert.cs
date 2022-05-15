using System.Threading.Tasks;
using Microsoft.JSInterop;
using WeeControl.Presentations.FunctionalService.Interfaces;

namespace WeeControl.Presentations.Wasm.Services;

public class DisplaySimpleAlert : IDisplayAlert
{
    private readonly IJSRuntime jsRuntime;

    public DisplaySimpleAlert(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
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