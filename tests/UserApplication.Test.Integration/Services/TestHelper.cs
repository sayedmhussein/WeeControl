using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.ApiApp.Domain.Contexts.Essential;
using WeeControl.Common.SharedKernel.Contexts.Essential.Entities;
using WeeControl.Common.SharedKernel.Services;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.AppService.Contexts.Essential.Interfaces;
using WeeControl.Frontend.AppService.Contexts.Essential.Models;
using WeeControl.Frontend.AppService.Interfaces;
using WeeControl.Frontend.AppService.Interfaces.GuiInterfaces;
using WeeControl.Frontend.Service;
using WeeControl.Frontend.Service.UnitTest;

namespace WeeControl.User.UserApplication.Test.Integration.Services;

public class TestHelper<T> : IDisposable
{
    public DeviceServiceMock DeviceMock { get; private set; }
    public IDevice Device { get; private set; }
    public T Service { get; private set; }

    public TestHelper(HttpClient httpClient)
    {
        DeviceMock = new DeviceServiceMock(nameof(T));
        Device = DeviceMock.GetObject(httpClient);
        
        var appServiceCollection = new ServiceCollection();
        appServiceCollection.AddApplicationServices();
        appServiceCollection.AddScoped(p => DeviceMock.GetObject(httpClient));
        
        using var scope = appServiceCollection.BuildServiceProvider().CreateScope();
        Service = scope.ServiceProvider.GetRequiredService<T>();
    }

    public void Dispose()
    {
        DeviceMock = null;
        Device = null;
        Service = default(T);
    }

    public async Task Authorize(string username, string password, string device = null)
    {
        using var helper = new TestHelper<IUserAuthorizationService>(Device.Server.HttpClient);
        if (device is not null)
            helper.DeviceMock.DeviceMock.Setup(x => x.DeviceId).Returns(device);

        await helper.Service.Login(new LoginModel() { UsernameOrEmail = username, Password = password});
        
        DeviceMock.InjectTokenToMock(await helper.Device.Security.GetTokenAsync());
    }

    private static string GetEncryptedPassword(string password)
    {
        return new PasswordSecurity().Hash(password);
    }
    
    public static UserDbo GetUserDboWithEncryptedPassword(string username, string password, string territory = "TST")
    {
        return new UserDbo(new UserEntity()
        {
            Username = username,
            Password = GetEncryptedPassword(password),
            MobileNo = "012345667",
            Email = (username + "@email.com").ToLower()
        });
    }
}