using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Services;
using WeeControl.User.UserApplication.ViewModels.User;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.User;

public class ForgotMyPasswordTests: IClassFixture<CustomWebApplicationFactory<Startup>>, System.IDisposable
{
    private ForgotMyPasswordViewModel vm;
    private readonly HttpClient httpClient;
    private DeviceServiceMock deviceMock;

    private readonly UserDbo normalUserDbo = 
        UserDbo.Create("normal@test.test", "normal", new PasswordSecurity().Hash("normal"),"TST");
    private readonly UserDbo lockedUserDbo = 
        UserDbo.Create("locked@test.test", "locked", new PasswordSecurity().Hash("locked"),"TST");

    public ForgotMyPasswordTests(CustomWebApplicationFactory<Startup> factory)
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
        
        
        deviceMock = new DeviceServiceMock(nameof(ForgotMyPasswordTests));
        
        var appServiceCollection = new ServiceCollection();
        appServiceCollection.AddViewModels();
        appServiceCollection.AddScoped(p => deviceMock.GetObject(httpClient));
        
        using var scope = appServiceCollection.BuildServiceProvider().CreateScope();
        vm = scope.ServiceProvider.GetRequiredService<ForgotMyPasswordViewModel>();
    }
    
    public void Dispose()
    {
        deviceMock = null;
        vm = null;
    }
    
    [Fact]
    public async void WhenSuccess()
    {
        vm.Email = normalUserDbo.Email;
        vm.Username = normalUserDbo.Username;
        
        await vm.RequestPasswordReset();
            
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Once);
    }
    
    [Fact]
    public async void WhenFail()
    {
        vm.Email = string.Empty;
        vm.Username = normalUserDbo.Username;
        
        await vm.RequestPasswordReset();
            
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async void WhenBusinessNotAllow_IsLockedUser()
    {
        vm.Email = lockedUserDbo.Email;
        vm.Username = lockedUserDbo.Username;
        
        await vm.RequestPasswordReset();
            
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Never);
    }
}