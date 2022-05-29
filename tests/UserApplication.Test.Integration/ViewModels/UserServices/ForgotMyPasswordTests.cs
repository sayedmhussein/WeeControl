using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Services;
using WeeControl.User.UserApplication.Test.Integration.ViewModels.Authorization;
using WeeControl.User.UserApplication.ViewModels.User;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.UserServices;

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
        await vm.RequestPasswordReset();
        vm.Email = normalUserDbo.Email;
        vm.Username = normalUserDbo.Username;
            
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async void WhenBusinessNotAllow_IsLockedUser()
    {
        await vm.RequestPasswordReset();
        vm.Email = lockedUserDbo.Email;
        vm.Username = lockedUserDbo.Username;
            
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Authentication.LoginPage, It.IsAny<bool>()), Times.Once);
    }
}