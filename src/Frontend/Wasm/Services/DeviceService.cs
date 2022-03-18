using System.Threading.Tasks;
using WeeControl.Common.UserSecurityLib.Interfaces;
using WeeControl.Frontend.Wasm.Interfaces;

namespace WeeControl.Frontend.Wasm.Services
{
    public class DeviceService : IClientDevice
    {
        public ILocalStorage LocalStorage { get; private set; }

        public string DeviceId => "This is device _Blazor_";
        
        public DeviceService(ILocalStorage localStorage)
        {
            LocalStorage = localStorage;
        }
        
        public  Task SaveTokenAsync(string token)
        {
            return LocalStorage.SetItem("token", token);
        }

        public Task<string> GetTokenAsync()
        {
            return LocalStorage.GetItem<string>("token");
        }

        public Task SaveUserNameTask(string userName)
        {
            return LocalStorage.SetItem("username", userName);
        }

        public Task SaveUserPhotoUrlAsync(string url)
        {
            return Task.CompletedTask;
        }

        public Task ClearUserDataAsync()
        {
            return LocalStorage.ClearItems();
        }

        
    }
}