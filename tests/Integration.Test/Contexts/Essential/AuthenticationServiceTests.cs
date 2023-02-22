using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.Test;
using WeeControl.Host.Test.ApiService;
using WeeControl.Host.WebApi;
using WeeControl.Host.WebApiService;
using WeeControl.Host.WebApiService.Contexts.User;

namespace WeeControl.Integration.Test.Contexts.Essential;

public class AuthenticationServiceTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public AuthenticationServiceTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Theory]
    [InlineData(CoreTestHelper.Username, CoreTestHelper.Password, true)]
    [InlineData(CoreTestHelper.Email, CoreTestHelper.Password, true)]
    [InlineData("username", "passwordX", false)]
    [InlineData("usernameX", "password", false)]
    [InlineData("usernameX", "passwordX", false)]
    public async void LoginTest(string usernameOrEmail, string password, bool success)
    {
        using var hostTestHelper = new HostTestHelper();
        var service = hostTestHelper.GetService<IAuthenticationService>(factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                CoreTestHelper.SeedDatabase(db);
            });
        }).CreateClient());

        await service.Login(LoginRequestDto.Create(usernameOrEmail, password));
        await service.UpdateToken("0000");
        
        hostTestHelper.GuiMock.Verify(x => 
            x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), 
            success? Times.Once : Times.Never);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("0")]
    [InlineData("00")]
    [InlineData("000")]
    [InlineData("1234")]
    public async void UpdateTokenTests(string otp)
    {
        using var h = new HostTestHelper();
        var service = h.GetService<IAuthenticationService>(factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                CoreTestHelper.SeedDatabase(db);
            });
        }).CreateClient());

        await service.Login(LoginRequestDto.Create(CoreTestHelper.Username, CoreTestHelper.Password));
        await service.UpdateToken(otp);
        
        h.GuiMock.Verify(x => 
                x.NavigateToAsync(ApplicationPages.Essential.OtpPage, It.IsAny<bool>()), 
                Times.Once);
        h.GuiMock.Verify(x => 
                x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), 
            Times.Never);
    }

    #region BusinessLogic
    [Fact]
    public async void WhenUserIsLocked()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IAuthenticationService>(factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                // var user = factory.GetUserDboWithEncryptedPassword("username", "password");
                // db.Users.Add(user);
                // user.Suspend("for testing");
                // db.SaveChanges();
            });
        }).CreateClient());


        await service.Login(LoginRequestDto.Create("username", "password"));

        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Never);

        helper.GuiMock.Verify(x =>
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
    }
    #endregion

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async void LogoutTests(bool isLoggedIn)
    {
        using var hostTestHelper = new HostTestHelper();
        var service = hostTestHelper.GetService<IAuthenticationService>(factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                CoreTestHelper.SeedDatabase(db);
            });
        }).CreateClient());

        if (isLoggedIn)
        {
            await service.Login(LoginRequestDto.Create("username", "password"));
            await service.UpdateToken("0000");
        }

        await service.Logout();
        
        hostTestHelper.StorageMock.Verify(x => x.ClearKeysValues(), Times.AtLeastOnce);
    }
}