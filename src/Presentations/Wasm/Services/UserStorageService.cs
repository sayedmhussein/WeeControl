using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using WeeControl.Presentations.FunctionalService.Enums;
using WeeControl.Presentations.FunctionalService.Interfaces;

namespace WeeControl.Presentations.Wasm.Services;

public class UserStorageService : IUserStorage
{
    private readonly IJSRuntime jsRuntime;

    public UserStorageService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }
    
    public Task SaveAsync(UserDataEnum name, string value)
    {
        return SetItem(Enum.GetName(typeof(UserDataEnum), name), value);
    }

    public Task<string> GetAsync(UserDataEnum name)
    {
        return GetItem<string>(Enum.GetName(typeof(UserDataEnum), name));
    }
    
    public Task ClearAsync()
    {
        return ClearItems();
    }
    
    private async Task<T> GetItem<T>(string key)
    {
        var json = await jsRuntime.InvokeAsync<string>("localStorage.getItem", key).ConfigureAwait(false);
        return json == null ? default : JsonSerializer.Deserialize<T>(json);
    }

    private async Task SetItem<T>(string key, T value)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value)).ConfigureAwait(false);
    }

    private async Task RemoveItem(string key)
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", key).ConfigureAwait(false);
    }

    private async Task ClearItems()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.clear").ConfigureAwait(false);
    }
}