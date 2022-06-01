using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Essential;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Services;
using WeeControl.User.UserApplication.ViewModels.Authentication;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.Authorization;

public class LogoutTests_Temp : IClassFixture<CustomWebApplicationFactory<Startup>>, System.IDisposable
{
    #region Preparation
    private LogoutViewModel vm;
    private readonly HttpClient httpClient;
    private DeviceServiceMock deviceMock;

    private readonly UserDbo normalUserDbo = 
        UserDbo.Create("normal@test.test", "normal", new PasswordSecurity().Hash("normal"),"TST");
    private readonly UserDbo lockedUserDbo = 
        UserDbo.Create("locked@test.test", "locked", new PasswordSecurity().Hash("locked"),"TST");

    public LogoutTests_Temp(CustomWebApplicationFactory<Startup> factory)
    {
        httpClient = factory.WithWebHostBuilder(builder =>
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
        
        deviceMock = new DeviceServiceMock(nameof(LogoutTests_Temp));
        
        var appServiceCollection = new ServiceCollection();
        appServiceCollection.AddViewModels();
        appServiceCollection.AddScoped(p => deviceMock.GetObject(httpClient));
        
        using var scope = appServiceCollection.BuildServiceProvider().CreateScope();
        vm = scope.ServiceProvider.GetRequiredService<LogoutViewModel>();
    }
    
    public void Dispose()
    {
        deviceMock = null;
        vm = null;
    }
    #endregion

    #region Success
    [Fact]
    public async void WhenSuccess()
    {
        var token = await LoginTests.GetNewToken(httpClient, normalUserDbo.Username, "normal", nameof(LogoutTests_Temp));
        deviceMock.InjectTokenToMock(token);

        await vm.LogoutAsync();
            
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Once);
    }
    

    #endregion

    #region UnAuthorized

    [Fact]
    public async void WhenUnAuthorized()
    {
        await vm.LogoutAsync();
            
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Once);
    }

    #endregion
    
}