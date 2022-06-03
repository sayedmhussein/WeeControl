using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Application.Essential;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Services;
using WeeControl.User.UserApplication.Interfaces;
using WeeControl.User.UserApplication.ViewModels.Authentication;
using WeeControl.WebApi;
using Xunit;

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
        using var helper = new TestHelper<LoginViewModel>(Device.Server.HttpClient);
        if (device is not null)
            helper.DeviceMock.DeviceMock.Setup(x => x.DeviceId).Returns(device);
        helper.ViewModel.UsernameOrEmail = username;
        helper.ViewModel.Password = password;

        // helper.DeviceMock.SecurityMock.Setup(x => x.UpdateTokenAsync(It.IsAny<string>()))
        //     .Callback((string tkn) => DeviceMock.InjectTokenToMock(tkn));
        
        await helper.ViewModel.LoginAsync();
        
        DeviceMock.InjectTokenToMock(await helper.Device.Security.GetTokenAsync());
    }

    public static string GetEncryptedPassword(string password)
    {
        return new PasswordSecurity().Hash(password);
    }
}

public class TestForTestClass : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public TestForTestClass(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }
    
    [Fact]
    public async void TestLoginWhenSuccess()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                db.Users.Add(UserDbo.Create(
                    "email@email.com", 
                    "username", 
                    TestHelper<object>.GetEncryptedPassword("password")));
                db.SaveChanges();
            });
        }).CreateClient();

        using var helper = new TestHelper<LogoutViewModel>(httpClient);
        await helper.Authorize("username", "password", "device");

        var token = await helper.Device.Security.GetTokenAsync();
        
        Assert.NotEmpty(token);
    }
    
    [Fact]
    public async void TestLoginWhenFailure()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                db.Users.Add(UserDbo.Create(
                    "email@email.com", 
                    "username", 
                    TestHelper<object>.GetEncryptedPassword("password")));
                db.SaveChanges();
            });
        }).CreateClient();

        using var helper = new TestHelper<LogoutViewModel>(httpClient);
        await helper.Authorize("username1", "password");

        var token = await helper.Device.Security.GetTokenAsync();
        
        Assert.Empty(token);
    }
}