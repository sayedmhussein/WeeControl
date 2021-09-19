using WeeControl.Frontend.CommonLib.Interfaces;

namespace WeeControl.Frontend.Wasm.Services
{
    public class DeviceService : IDevice
    {
        private static string _token;
        private static string _fullName;
        

        public ILocalStorage LocalStorage { get; private set; }
        public string DeviceId => "This is device _Blazor_";

        public DeviceService(ILocalStorage localStorage)
        {
            LocalStorage = localStorage;
        }
        
        // [Obsolete]
        // public string Token
        // {
        //     get
        //     {
        //         if (string.IsNullOrEmpty(_token))
        //             Task.Run(async () => _token = await GetItem<string>(nameof(Token)));
        //         return _token;
        //     }
        //     set
        //     {
        //         Task.Run(() => SetItem(nameof(Token), value));
        //         _token = value;
        //     }
        // }

        // [Obsolete]
        // public string FullName
        // {
        //     get
        //     {
        //         if (string.IsNullOrEmpty(_fullName))
        //             Task.Run(async () => _fullName = await GetItem<string>(nameof(FullName)));
        //         return _fullName;
        //     }
        //     set
        //     {
        //         Task.Run(async () => await SetItem(nameof(FullName), value));
        //         _fullName = value;
        //     }
        // }
        
        public string PhotoUrl { get; set; }

        
    }
}