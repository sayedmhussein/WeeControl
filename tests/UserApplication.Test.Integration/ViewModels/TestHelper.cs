using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.User.UserApplication.Interfaces;
using WeeControl.User.UserApplication.Test.Integration.ViewModels.Authorization;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels;

public class TestHelper<T> : IDisposable
{
    public DeviceServiceMock DeviceMock { get; private set; }
    public IDevice Device { get; private set; }
    public T ViewModel { get; private set; }

    public TestHelper(HttpClient httpClient)
    {
        DeviceMock = new DeviceServiceMock(nameof(T));
        Device = DeviceMock.GetObject(httpClient);
        
        var appServiceCollection = new ServiceCollection();
        appServiceCollection.AddViewModels();
        appServiceCollection.AddScoped(p => DeviceMock.GetObject(httpClient));
        
        using var scope = appServiceCollection.BuildServiceProvider().CreateScope();
        ViewModel = scope.ServiceProvider.GetRequiredService<T>();
    }

    public void Dispose()
    {
        DeviceMock = null;
        Device = null;
        ViewModel = default(T);
    }

    public async Task Authorize(string username, string password)
    {
        var token = await LoginTests.GetNewToken(Device.Server.HttpClient, username, password, nameof(T));
        DeviceMock.InjectTokenToMock(token);
    }
}