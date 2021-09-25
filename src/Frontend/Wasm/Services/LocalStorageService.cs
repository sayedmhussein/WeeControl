using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using WeeControl.Frontend.Wasm.Interfaces;

namespace WeeControl.Frontend.Wasm.Services
{
    public class LocalStorageService : ILocalStorage
    {
        private readonly IJSRuntime jsRuntime;

        public LocalStorageService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }
        
        public async Task<T> GetItem<T>(string key)
        {
            var json = await jsRuntime.InvokeAsync<string>("localStorage.getItem", key).ConfigureAwait(false);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }

        public async Task SetItem<T>(string key, T value)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value)).ConfigureAwait(false);
        }

        public async Task RemoveItem(string key)
        {
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", key).ConfigureAwait(false);
        }

        public async Task ClearItems()
        {
            await jsRuntime.InvokeVoidAsync("localStorage.clear").ConfigureAwait(false);
        }
    }
}