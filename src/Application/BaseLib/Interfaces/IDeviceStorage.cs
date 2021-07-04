using System;
using System.Threading.Tasks;

namespace WeeControl.Applications.BaseLib.Interfaces
{
    public interface IDeviceStorage
    {
        Task SaveTokenAsync(string token);
        Task<string> GetTokenAsync();
        Task ClearTokenAsync();

        string FullUserName { get; set; }
    }
}
