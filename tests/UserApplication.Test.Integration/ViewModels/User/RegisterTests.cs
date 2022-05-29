using System;
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

public class RegisterTests : IClassFixture<CustomWebApplicationFactory<Startup>>, System.IDisposable
{
    private RegisterViewModel vm;
    private readonly HttpClient httpClient;
    private DeviceServiceMock deviceMock;

    
    private readonly UserDbo normalUserDbo = 
        UserDbo.Create("normal@test.test", "normal", new PasswordSecurity().Hash("normal"),"TST");
    private readonly UserDbo lockedUserDbo = 
        UserDbo.Create("locked@test.test", "locked", new PasswordSecurity().Hash("locked"),"TST");

    
    public RegisterTests(CustomWebApplicationFactory<Startup> factory)
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
        vm = scope.ServiceProvider.GetRequiredService<RegisterViewModel>();
    }

    public void Dispose()
    {
        deviceMock = null;
        vm = null;
    }
    
    [Fact]
    public async void WhenSuccess()
    {
        vm.Email = "email@email.com";
        vm.Username = "someUsername";
        vm.Password = "somePassword";
        
        await vm.RegisterAsync();

        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Once);
    }
    
    [Fact]
    public async void WhenBusinessNotAllow_DublicateEmail()
    {
        vm.Email = normalUserDbo.Email;
        vm.Username = "someUsername";
        vm.Password = "somePassword";
        
        await vm.RegisterAsync();
            
        deviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);;
    }
    
    [Fact]
    public async void WhenBusinessNotAllow_DublicateUsername()
    {
        vm.Email = "email@email.com";
        vm.Username = normalUserDbo.Username;
        vm.Password = "somePassword";
        
        await vm.RegisterAsync();
            
        deviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);;
    }
}