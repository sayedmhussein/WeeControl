using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Domain.Contexts;
using WeeControl.Domain.Contexts.Essential;
using WeeControl.Frontend.ApplicationService;
using WeeControl.Frontend.ApplicationService.Interfaces;
using WeeControl.Frontend.ApplicationService.UnitTest;
using WeeControl.SharedKernel.Services;

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

    public async Task Authorize(string username, string password, string device = null)
    {
        using var helper = new TestHelper<AuthorizationViewModel>(Device.Server.HttpClient);
        if (device is not null)
            helper.DeviceMock.DeviceMock.Setup(x => x.DeviceId).Returns(device);
        helper.ViewModel.UsernameOrEmail = username;
        helper.ViewModel.Password = password;

        await helper.ViewModel.Login();
        
        DeviceMock.InjectTokenToMock(await helper.Device.Security.GetTokenAsync());
    }

    public static string GetEncryptedPassword(string password)
    {
        return new PasswordSecurity().Hash(password);
    }
    
    public static UserDbo GetUserDboWithEncryptedPassword(string username, string password, string territory = "TST")
    {
        return UserDbo.Create(
            nameof(UserDbo.FirstName), 
            nameof(UserDbo.LastName), 
            (username + "@email.com").ToLower(), 
            username.ToLower(),
            GetEncryptedPassword(password), 
            "012345667", 
            territory, "EGP");
    }
}