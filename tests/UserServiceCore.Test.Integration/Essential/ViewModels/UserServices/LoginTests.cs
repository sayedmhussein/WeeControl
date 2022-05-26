using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Services;
using WeeControl.User.UserServiceCore;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.ViewModels.Authentication;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.ServiceCore.Test.Integration.Essential.ViewModels.UserServices;

public class LoginTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
{
    #region static
    public static async Task<string> LoginAsync(HttpClient client, string username, string password, string device)
    {
        var token = string.Empty;
    
        var mocks = new DeviceServiceMock(device);
        mocks.StorageMock.Setup(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()))
            .Callback((UserDataEnum en, string tkn) => token = tkn);
        
        var appServiceCollection = new ServiceCollection();
        appServiceCollection.AddUserServiceCore();
        appServiceCollection.AddScoped(p => mocks.GetObject(client));
        
        using var scope = appServiceCollection.BuildServiceProvider().CreateScope();
        var vm = scope.ServiceProvider.GetRequiredService<LoginViewModel>();
        vm.UsernameOrEmail = username;
        vm.Password = password;
        await vm.LoginAsync();
            
        Assert.NotEmpty(token);
            
        return token;
    }
    #endregion

    private LoginViewModel vm;
    private DeviceServiceMock deviceMock;
    private readonly CustomWebApplicationFactory<Startup> factory;
    
    private readonly UserDbo normalUserDbo = 
        UserDbo.Create("normal@test.test", "normal", new PasswordSecurity().Hash("normal"),"TST");
    private readonly UserDbo lockedUserDbo = 
        UserDbo.Create("locked@test.test", "locked", new PasswordSecurity().Hash("locked"),"TST");
    
    public LoginTests(CustomWebApplicationFactory<Startup> factory)
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            lockedUserDbo.Suspend("For Test Only");
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                db.Users.Add(normalUserDbo);
                
                db.Users.Add(lockedUserDbo);
                db.SaveChanges();
            });
        }).CreateClient();
        
        deviceMock = new DeviceServiceMock(nameof(LoginTests));
        
        var appServiceCollection = new ServiceCollection();
        appServiceCollection.AddUserServiceCore();
        appServiceCollection.AddScoped(p => deviceMock.GetObject(httpClient));
        
        using var scope = appServiceCollection.BuildServiceProvider().CreateScope();
        vm = scope.ServiceProvider.GetRequiredService<LoginViewModel>();
        
        this.factory = factory;
    }

    public void Dispose()
    {
        deviceMock = null;
        vm = null;
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "password")]
    [InlineData("username", "")]
    [InlineData("usernotexist", "usernotexist")]
    public async void TestsForFailedScenarios(string username, string password)
    {
        vm.UsernameOrEmail = username;
        vm.Password = password;
        
        await vm.LoginAsync();
            
        deviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
        
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
    }
        
    [Fact]
    public async void WhenUserIsLocked()
    {
        vm.UsernameOrEmail = lockedUserDbo.Username;
        vm.Password = lockedUserDbo.Username;
        
        await vm.LoginAsync();
        
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
        
        deviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
    {
        vm.UsernameOrEmail = normalUserDbo.Username;
        vm.Password = normalUserDbo.Username;
        
        await vm.LoginAsync();
        
        deviceMock.StorageMock.Verify(x => x.
            SaveAsync(UserDataEnum.Token, It.IsAny<string>()));
        deviceMock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Once);
    }
    
    [Fact]
    public async void TestForStaticFunction()
    {
        const string email = "email@email.com";
        const string username = "username";
        const string password = "password";
        
        var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    using var scope = services.BuildServiceProvider().CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                    db.Users.Add(UserDbo.Create(email, username, new PasswordSecurity().Hash(password)));
                    db.SaveChanges();
                });
            })
            .CreateClient();
    
        var token = await LoginAsync(client, username, password, "device");
        
        Assert.NotEmpty(token);
    }

    [Obsolete("Unused function!!!")]
    private HttpClient GetHttpClientForTesting(HttpStatusCode statusCode, HttpContent content)
    {
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = statusCode, 
            Content = content
        };
        
        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
        
        return new HttpClient(httpMessageHandlerMock.Object);
    }
}