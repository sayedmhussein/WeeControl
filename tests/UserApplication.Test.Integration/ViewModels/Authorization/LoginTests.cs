using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.DataTransferObjects.Authentication;
using WeeControl.SharedKernel.Services;
using WeeControl.User.UserApplication.ViewModels.Authentication;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.Authorization;

public class LoginTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
{
    #region static
    public static async Task<string> GetNewToken(HttpClient client, string username, string password, string device)
    {
        var token = string.Empty;
    
        var mocks = new DeviceServiceMock(device);
        mocks.SecurityMock.Setup(x => x.UpdateTokenAsync(It.IsAny<string>()))
            .Callback((string tkn) => token = tkn);
        
        var appServiceCollection = new ServiceCollection();
        appServiceCollection.AddViewModels();
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

    #region Preparation
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
        appServiceCollection.AddViewModels();
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
    #endregion

    #region Success
    [Fact]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
    {
        vm.UsernameOrEmail = normalUserDbo.Username;
        vm.Password = normalUserDbo.Username;
        
        await vm.LoginAsync();
        
        deviceMock.SecurityMock.Verify(x => x.
            UpdateTokenAsync(It.IsAny<string>()));
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
    
        var token = await GetNewToken(client, username, password, "device");
        
        Assert.NotEmpty(token);
    }
    #endregion

    #region Unauthorized

    

    #endregion

    #region BusinessLogic
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
    #endregion
}