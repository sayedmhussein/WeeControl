using WeeControl.Frontend.AppService.Internals.Interfaces;

namespace WeeControl.Frontend.AppService.Internals.Services;

internal class ServiceDataService : IServiceData
{
    private readonly IDeviceSecurity deviceSecurity;

    public ServiceDataService(IDeviceSecurity deviceSecurity)
    {
        this.deviceSecurity = deviceSecurity;
    }
    
    public Task<bool> IsAuthenticated()
    {
        return deviceSecurity.IsAuthenticatedAsync();
    }
}