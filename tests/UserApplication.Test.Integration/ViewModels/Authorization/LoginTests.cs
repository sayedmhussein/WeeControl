using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Essential;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Services;
using WeeControl.User.UserApplication.ViewModels.Authentication;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.Authorization;

public class LoginTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    #region static
    [Obsolete("Use Authorize() in test helper")]
    public static async Task<string> GetNewToken(HttpClient client, string username, string password, string device)
    {
        var mocks = new DeviceServiceMock(device);
        var mockObject = mocks.GetObject(client);

        var appServiceCollection = new ServiceCollection();
        appServiceCollection.AddViewModels();
        appServiceCollection.AddScoped(p => mockObject);
        
        using var scope = appServiceCollection.BuildServiceProvider().CreateScope();
        var vm = scope.ServiceProvider.GetRequiredService<LoginViewModel>();
        vm.UsernameOrEmail = username;
        vm.Password = password;
        await vm.LoginAsync();

        var token = await mockObject.Security.GetTokenAsync();
        Assert.NotEmpty(token);
            
        return token;
    }
    #endregion

    #region Preparation
    private readonly CustomWebApplicationFactory<Startup> factory;
    
    public LoginTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }
    #endregion

    #region Success
    [Theory]
    [InlineData("username")]
    [InlineData("email@email.com")]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode(string usernameOrEmail)
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
        
        using var helper = new TestHelper<LoginViewModel>(httpClient);
        helper.ViewModel.UsernameOrEmail = usernameOrEmail;
        helper.ViewModel.Password = "password";

        await helper.ViewModel.LoginAsync();
        
        helper.DeviceMock.SecurityMock.Verify(x => x.
            UpdateTokenAsync(It.IsAny<string>()));
        helper.DeviceMock.NavigationMock.Verify(x => x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Once);
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

    #region BusinessLogic
    [Fact]
    public async void WhenUserIsLocked()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var user = UserDbo.Create(
                    "email@email.com",
                    "username",
                    TestHelper<object>.GetEncryptedPassword("password"));
                db.Users.Add(user);
                user.Suspend("for testing");
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<LoginViewModel>(httpClient);
        helper.ViewModel.UsernameOrEmail = "username";
        helper.ViewModel.Password = "password";

        await helper.ViewModel.LoginAsync();
        
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index, It.IsAny<bool>()), Times.Never);
        
        helper.DeviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
    }
    #endregion
}