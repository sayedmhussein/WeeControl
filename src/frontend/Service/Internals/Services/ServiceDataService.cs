using System.Security.Claims;
using WeeControl.Common.SharedKernel;
using WeeControl.Frontend.AppService.Internals.Interfaces;

namespace WeeControl.Frontend.AppService.Internals.Services;

internal class ServiceDataService : IServiceData
{
    private readonly IDeviceSecurity deviceSecurity;
    private readonly IServerOperation serverOperation;

    public ServiceDataService(IDeviceSecurity deviceSecurity, IServerOperation serverOperation)
    {
        this.deviceSecurity = deviceSecurity;
        this.serverOperation = serverOperation;
        deviceSecurity.TokenChanged += DeviceSecurityOnTokenChanged;
    }

    private void DeviceSecurityOnTokenChanged(object? sender, string e)
    {
        AuthenticationChanged?.Invoke(this, !string.IsNullOrEmpty(e));
    }

    public Task<bool> IsAuthenticated()
    {
        return deviceSecurity.IsAuthenticatedAsync();
    }

    public async Task<bool> IsConnectedToServer()
    {
        var response = await serverOperation.GetResponseMessage(HttpMethod.Head, new Version("1.0"), ApiRouting.AuthorizationRoute);
        return response.IsSuccessStatusCode;
    }

    public Task<ClaimsPrincipal> GetClaimPrincipal()
    {
        return deviceSecurity.GetClaimsPrincipal();
    }

    public event EventHandler<bool>? AuthenticationChanged;
}