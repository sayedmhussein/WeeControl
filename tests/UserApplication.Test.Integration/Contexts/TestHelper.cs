using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using WeeControl.ApiApp.Domain.Contexts.Essential;
using WeeControl.Common.SharedKernel.Contexts.Temporary.Entities;
using WeeControl.Common.SharedKernel.Services;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.Service.UnitTest;

namespace WeeControl.User.UserApplication.Test.Integration.Contexts;

public class TestHelper<T> : IDisposable where T : class
{
    public Mock<IDeviceData> DeviceMock { get; private set; }
    public IDeviceData Device { get; private set; }
    public T Service { get; private set; }

    public TestHelper(HttpClient httpClient)
    {
        var helper = new TestHelper(nameof(TestHelper));
        Service = helper.GetService<T>(httpClient);
        DeviceMock = helper.DeviceMock;
        Device = DeviceMock.Object;
    }

    public void Dispose()
    {
        DeviceMock = null;
        Device = null;
        Service = default(T);
    }

    public async Task Authorize(string username, string password, string device = null)
    {
        // using var helper = new TestHelper<IAuthorizationService>(Device.Server.HttpClient);
        // if (device is not null)
        //     helper.DeviceMock.DeviceMock.Setup(x => x.DeviceId).Returns(device);
        //
        // await helper.Service.Login(username, password);
        //
        // DeviceMock.InjectTokenToMock(await helper.Device.Security.GetTokenAsync());
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