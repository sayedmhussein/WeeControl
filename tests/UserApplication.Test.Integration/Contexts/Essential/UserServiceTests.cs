using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Threading.Tasks;
using WeeControl.Core.DataTransferObject.Contexts.Temporary.User;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Frontend.AppService;
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;
using WeeControl.Frontend.AppService.GuiInterfaces.Home;
using WeeControl.Frontend.Service.UnitTest;
using WeeControl.Host.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.Contexts.Essential;

public class UserServiceTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public UserServiceTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    #region VerifyAuthentication

    [Fact]
    public async void VerifyAuthenticationTests()
    {
        using var helper = await GetTestHelperWithStandardUsername();
        var service = helper.GetService<IHomeService>();

        var result = service.VerifyAuthentication();


    }
    #endregion

    #region ChangeMyPassword()
    [Fact]
    public async void ChangeMyPassword_WhenSuccess()
    {
        using var helper = await GetTestHelperWithStandardUsername();
        var service = helper.GetService<IHomeService>();

        await service.ChangeMyPassword(new UserPasswordChangeRequestDto()
        {
            OldPassword = "password",
            NewPassword = "someNewPassword",
            ConfirmPassword = "someNewPassword"
        });

        helper.DeviceMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.SplashPage, It.IsAny<bool>()), Times.AtLeastOnce);
    }

    [Fact]
    public async void ChangeMyPassword_WhenUnauthorized()
    {
        using var helper = await GetTestHelperWithStandardUsername();
        await helper.GetService<IAuthorizationService>().Logout();
        var service = helper.GetService<IHomeService>();

        await service.ChangeMyPassword(new UserPasswordChangeRequestDto()
        {
            OldPassword = "password",
            NewPassword = "someNewPassword",
            ConfirmPassword = "someNewPassword"
        });

        helper.DeviceMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.DeviceMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.HomePage, It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async void ChangeMyPassword_WhenBusinessNotAllow_InvalidPassword()
    {
        using var helper = await GetTestHelperWithStandardUsername();
        var service = helper.GetService<IHomeService>();

        await service.ChangeMyPassword(new UserPasswordChangeRequestDto()
        {
            OldPassword = "invalid password",
            NewPassword = "someNewPassword",
            ConfirmPassword = "someNewPassword"
        });

        helper.DeviceMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.DeviceMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.HomePage, It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async void ChangeMyPassword_WhenUserIsLocked()
    {
        using var helper = await GetTestHelperWithStandardUsername();
        var service = helper.GetService<IHomeService>();

        await service.ChangeMyPassword(new UserPasswordChangeRequestDto()
        {
            OldPassword = "password",
            NewPassword = "someNewPassword",
            ConfirmPassword = "someNewPassword"
        });

        helper.DeviceMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.DeviceMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.HomePage, It.IsAny<bool>()), Times.Never);
    }
    #endregion

    #region RequestPasswordReset()
    [Fact]
    public async void RequestPasswordReset_WhenSuccess()
    {
        using var helper = await GetTestHelperWithStandardUsername();
        var service = helper.GetService<IHomeService>();

        await service.RequestPasswordReset(new UserPasswordResetRequestDto()
        {
            Email = "email@email.com",
            Username = "username"
        });

        helper.DeviceMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.UserPage, It.IsAny<bool>()), Times.Once);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("email@email.com", "")]
    [InlineData("", "username")]
    public async void RequestPasswordReset_WhenInvalidEmailAndUsernameMatchingOrNotExist(string email, string username)
    {
        using var helper = await GetTestHelperWithStandardUsername();
        var service = helper.GetService<IHomeService>();

        await service.RequestPasswordReset(new UserPasswordResetRequestDto()
        {
            Email = email,
            Username = username
        });

        helper.DeviceMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.UserPage, It.IsAny<bool>()), Times.Never);
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
                var user = factory.GetUserDboWithEncryptedPassword("username", "password");
                user.Suspend("for testing");
                db.Users.Add(user);
                db.SaveChanges();
            });
        }).CreateClient();

        using var helper = new TestHelper(nameof(RequestPasswordReset_WhenBusinessNotAllow_IsLockedUser));
        var service = helper.GetService<IHomeService>(httpClient);

        await service.RequestPasswordReset(new UserPasswordResetRequestDto()
        {
            Email = "username@email.com",
            Username = "username"
        });

        helper.DeviceMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.UserPage, It.IsAny<bool>()), Times.Never);
    }
    #endregion

    #region Register()
    [Theory]
    [InlineData("someEmail@email.com", "someUsername", "0123456789")]
    public async void Register_WhenSuccess(string email, string username, string mobileNo)
    {
        var model = new RegisterCustomerDto
        {
            Personal =
            {
                // FirstName = username,
                // LastName = username,
                // Nationality = "EGP"
            },
            User =
            {
                Email = email,
                Username = username,
                Password = "somePassword",
                PasswordConfirmation = "somePassword",
                MobileNo = mobileNo
            },
            Customer =
            {
                CountryCode = "EGP"
            }
        };

        using var helper = await GetTestHelperWithStandardUsername();
        var service = helper.GetService<IHomeService>();

        await service.Register(model);

        helper.DeviceMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.SplashPage, It.IsAny<bool>()), Times.AtLeastOnce);
    }

    [Theory]
    [InlineData("username@email.com", "username", "0123456")]
    [InlineData("username1@email.com1", "username", "0123456")]
    [InlineData("username@email.com", "username1", "0123456")]
    public async void Register_WhenBusinessNotAllow_ExistingUser(string email, string username, string mobileNo)
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var user = factory.GetUserDboWithEncryptedPassword("username", "password");
                db.Users.Add(user);
                user.Suspend("for testing");
                db.SaveChanges();
            });
        }).CreateClient();

        using var helper = new TestHelper(nameof(RequestPasswordReset_WhenBusinessNotAllow_IsLockedUser));
        var service = helper.GetService<IHomeService>(httpClient);

        var model = new RegisterCustomerDto
        {
            Personal =
            {
                // FirstName = username,
                // LastName = username,
                // Nationality = "EGP"
            },
            User =
            {
                Email = email,
                Username = username,
                Password = "somePassword",
                MobileNo = mobileNo
            },
            Customer =
            {
                CountryCode = "EGP"
            }
        };

        await service.Register(model);

        helper.DeviceMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.DeviceMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.SplashPage, It.IsAny<bool>()), Times.Never); ;
    }
    #endregion

    private async Task<TestHelper> GetTestHelperWithStandardUsername()
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var user = factory.GetUserDboWithEncryptedPassword("username", "password");
                db.Users.Add(user);
                db.SaveChanges();
            });
        }).CreateClient();

        var helper = new TestHelper(nameof(ChangeMyPassword_WhenSuccess));
        helper.GetService<IAuthorizationService>(httpClient);
        await factory.Authorize(helper, "username", "password");
        return helper;
    }
}