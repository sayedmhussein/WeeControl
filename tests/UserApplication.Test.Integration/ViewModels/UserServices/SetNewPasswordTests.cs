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

public class SetNewPasswordTests : IClassFixture<CustomWebApplicationFactory<Startup>>, System.IDisposable
{
    private SetNewPasswordViewModel vm;
    private readonly HttpClient httpClient;
    private DeviceServiceMock deviceMock;

    private readonly UserDbo normalUserDbo = 
        UserDbo.Create("normal@test.test", "normal", new PasswordSecurity().Hash("normal"),"TST");
    private readonly UserDbo lockedUserDbo = 
        UserDbo.Create("locked@test.test", "locked", new PasswordSecurity().Hash("locked"),"TST");

    
    public SetNewPasswordTests(CustomWebApplicationFactory<Startup> factory)
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
        
        deviceMock = new DeviceServiceMock(nameof(SetNewPasswordTests));
        
        var appServiceCollection = new ServiceCollection();
        appServiceCollection.AddViewModels();
        appServiceCollection.AddScoped(p => deviceMock.GetObject(httpClient));
        
        using var scope = appServiceCollection.BuildServiceProvider().CreateScope();
        vm = scope.ServiceProvider.GetRequiredService<SetNewPasswordViewModel>();
    }
    
    public void Dispose()
    {
        deviceMock = null;
        vm = null;
    }
    
    [Fact]
    public async void WhenSuccess()
    {
        var token = await LoginTests.GetNewToken(httpClient, 
            normalUserDbo.Username, 
            "normal", nameof(SetNewPasswordTests));
        deviceMock.InjectTokenToMock(token);

        vm.OldPassword = "normal";
        vm.NewPassword = "someNewPassword";
        vm.ConfirmNewPassword = "someNewPassword";

        await vm.ChangeMyPassword();
            
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Once);
    }
    
    [Fact]
    public async void WhenUnauthorized()
    {
        vm.OldPassword = "normal";
        vm.NewPassword = "someNewPassword";
        vm.ConfirmNewPassword = "someNewPassword";

        await vm.ChangeMyPassword();
            
        deviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
    }
    
    [Fact]
    public async void WhenBusinessNotAllow_Locked()
    {
        var token = await LoginTests.GetNewToken(httpClient, 
            lockedUserDbo.Username, 
            "locked", nameof(SetNewPasswordTests));
        
        Assert.Empty(token);
    }

    
}