using Microsoft.Extensions.DependencyInjection;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.Test;
using WeeControl.Host.Test.ApiService;
using WeeControl.Host.WebApi;
using WeeControl.Host.WebApiService;
using WeeControl.Host.WebApiService.Contexts.Essentials;

namespace WeeControl.Integration.Test.Contexts.Essential;

public class AuthenticationServiceTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public AuthenticationServiceTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async void WhenLoginWithoutOtp_ShouldAdminShouldNotHave(bool refresh)
    {
        using var hostTestHelper = new HostTestHelper();
        var client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                CoreTestHelper.SeedDatabase(db);
            });
        }).CreateClient();
        
        var service = hostTestHelper.GetService<IAuthenticationService>(client);
        await service.Login(LoginRequestDto.Create(CoreTestHelper.Username, CoreTestHelper.Password));

        if (refresh)
        {
            await service.UpdateToken("0000");
        }

        var bla = hostTestHelper.GetService<ISecurity>(client);
        var claims = (await bla.GetClaimsPrincipal()).Claims;
        
        Assert.NotEmpty(claims);

        if (refresh)
        {
            Assert.Contains(CoreTestHelper.ClaimTypeExample, claims.Select(x => x.Type));
            Assert.Contains(CoreTestHelper.ClaimValueExample, claims.Select(x => x.Value));
        }
        else
        {
            Assert.DoesNotContain(CoreTestHelper.ClaimTypeExample, claims.Select(x => x.Type));
            Assert.DoesNotContain(CoreTestHelper.ClaimValueExample, claims.Select(x => x.Value));
        }
        
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
    
    #region RequestPasswordReset()
    
    [Fact]
    public async void RequestPasswordReset_WhenSuccess()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                CoreTestHelper.SeedDatabase(db);
            });
        }).CreateClient();
        
        using var helper = new HostTestHelper();
        var service = helper.GetService<IAuthenticationService>(httpClient);

        await service.RequestPasswordReset(new UserPasswordResetRequestDto()
        {
            Email = CoreTestHelper.Email,
            Username = CoreTestHelper.Username
        });

        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Once);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("email@email.com", "")]
    [InlineData("", "username")]
    public async void RequestPasswordReset_WhenInvalidEmailAndUsernameMatchingOrNotExist(string email, string username)
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IAuthenticationService>();

        await service.RequestPasswordReset(new UserPasswordResetRequestDto()
        {
            Email = email,
            Username = username
        });

        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async void RequestPasswordReset_WhenBusinessNotAllow_IsLockedUser()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                CoreTestHelper.SeedDatabase(db);
                var user = db.Users.First();
                user.Suspend("for testing");
                db.SaveChanges();
            });
        }).CreateClient();

        using var helper = new HostTestHelper();
        var service = helper.GetService<IAuthenticationService>(httpClient);

        await service.RequestPasswordReset(new UserPasswordResetRequestDto()
        {
            Email = CoreTestHelper.Email,
            Username = CoreTestHelper.Username
        });

        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Never);
    }
    #endregion
}