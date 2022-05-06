using System;
using WeeControl.Frontend.ServiceLibrary.Interfaces;

namespace WeeControl.Frontend.Wasm.Services
{
    public class UserDeviceService : IUserDevice
    {
        public string DeviceId => "This is device _Blazor_";
        public DateTime TimeStamp => DateTime.UtcNow;
        public double? Latitude => null;
        public double? Longitude => null;
    }
}