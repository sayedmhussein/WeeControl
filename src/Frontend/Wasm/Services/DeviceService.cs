using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeeControl.Common.SharedKernel.Enums;
using WeeControl.Common.SharedKernel.Interfaces;
using WeeControl.Frontend.Wasm.Interfaces;

namespace WeeControl.Frontend.Wasm.Services
{
    public class DeviceService : IUserDevice
    {
        public ILocalStorage LocalStorage { get; private set; }

        public string DeviceId => "This is device _Blazor_";
        public DateTime TimeStamp => DateTime.UtcNow;
        public double? Latitude { get; }
        public double? Longitude { get; }

        public string ServerBaseAddress
        {
            get => "https://localhost:5001/";
            set => _ = value;
        }

        public HttpClient HttpClient { get; }

        public DeviceService(ILocalStorage localStorage)
        {
            LocalStorage = localStorage;
        }

        public Task SaveAsync(UserDataEnum name, string value)
        {
            return LocalStorage.SetItem(nameof(name), value);
        }

        public Task<string> GetAsync(UserDataEnum name)
        {
            return LocalStorage.GetItem<string>(nameof(name));
        }

        public Task SaveTokenAsync(string token)
        {
            return LocalStorage.SetItem("token", token);
        }

        public Task<string> GetTokenAsync()
        {
            return LocalStorage.GetItem<string>("token");
        }

        public Task ClearAsync()
        {
            return LocalStorage.ClearItems();
        }

        string IUserDevice.DeviceId => DeviceId;
    }
}