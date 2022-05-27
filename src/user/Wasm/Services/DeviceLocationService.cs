using System.Threading.Tasks;
using Microsoft.JSInterop;
using WeeControl.User.UserApplication.Interfaces;

namespace WeeControl.User.Wasm.Services;

public class DeviceLocationService : IDeviceLocation
{
    private readonly IJSRuntime jsRuntime;

    public DeviceLocationService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }
    
    public Task<(double? Latitude, double? Longitude)> GetAccurateLocationAsync()
    {
        return Task.FromResult<(double?, double?)>((null, null));
    }

    public Task<(double? Latitude, double? Longitude)> GetLastKnownLocationAsync()
    {
        return Task.FromResult<(double?, double?)>((null, null));
    }

    public bool LocationIsAvailable()
    {
        return false;
    }
}