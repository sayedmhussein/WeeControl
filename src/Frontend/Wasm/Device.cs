using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using WeeControl.Frontend.CommonLib.Interfaces;

namespace WeeControl.Frontend.Wasm
{
    public class Device : IDevice
    {
        private static string token;
        private readonly IJSRuntime jsRuntime;
        private readonly IJSUnmarshalledRuntime jsUnmarshalledRuntime;
        public string DeviceId => "This is device _Blazor_";

        public string Token
        {
            get
            {
                if (string.IsNullOrEmpty(token))
                    Task.Run(async () => token = await GetItem<string>(nameof(Token)));
                return token;
            }
            set
            {
                Task.Run(() => SetItem(nameof(Token), value));
                token = value;
            }
        }

        public string FullName
        {
            get => Task.Run(async () => await GetItem<string>(nameof(FullName))).GetAwaiter().GetResult();
            set => Task.Run(async () => await SetItem(nameof(FullName), value));
        }

        public string PhotoUrl { get; set; }

        public Device(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
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
    }
}