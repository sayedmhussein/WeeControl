using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.Domain.Interfaces;
using WeeControl.Core.Test;
using WeeControl.Host.Test.ApiService;
using WeeControl.Host.WebApi;
using WeeControl.Host.WebApiService;
using WeeControl.Host.WebApiService.Contexts.Essentials;

namespace WeeControl.Integration.Test.Contexts.Essential;

public class UserServiceTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public UserServiceTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    #region ChangeMyPassword()
    
    [Fact]
    public async void ChangeMyPassword_WhenSuccess()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IUserService>(GetHttpClient());
        await factory.Authorize(helper, CoreTestHelper.Username, CoreTestHelper.Password);

        await service.ChangePassword(new UserPasswordChangeRequestDto()
        {
            OldPassword = CoreTestHelper.Password, NewPassword = "NewPassword"
        });

        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Once);
    }

    [Fact]
    public async void ChangeMyPassword_WhenUnauthorized()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IUserService>(GetHttpClient());

        await service.ChangePassword(new UserPasswordChangeRequestDto()
        {
            OldPassword = CoreTestHelper.Password, NewPassword = "NewPassword"
        });

        helper.GuiMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Never);
    }

    [Fact]
    public async void ChangeMyPassword_WhenBusinessNotAllow_InvalidPassword()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IUserService>(GetHttpClient());
        await factory.Authorize(helper, CoreTestHelper.Username, CoreTestHelper.Password);

        await service.ChangePassword(new UserPasswordChangeRequestDto()
        {
            OldPassword = "Invalid Password", NewPassword = "NewPassword", ConfirmPassword = "NewPassword"
        });

        helper.GuiMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.AtMostOnce);
    }

    [Fact]
    public async void ChangeMyPassword_WhenUserIsLocked()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IUserService>(GetHttpClient(lockUser:true));
        await factory.Authorize(helper, CoreTestHelper.Username, CoreTestHelper.Password);

        await service.ChangePassword(new UserPasswordChangeRequestDto()
        {
            OldPassword = CoreTestHelper.Password, NewPassword = "NewPassword"
        });
        

        helper.GuiMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Never);
    }
    #endregion

    #region RequestPasswordReset()
    
    [Fact]
    public async void RequestPasswordReset_WhenSuccess()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IUserService>(GetHttpClient());

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
        var service = helper.GetService<IUserService>();

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
        var service = helper.GetService<IUserService>(httpClient);

        await service.RequestPasswordReset(new UserPasswordResetRequestDto()
        {
            Email = CoreTestHelper.Email,
            Username = CoreTestHelper.Username
        });

        helper.GuiMock.Verify(x =>
            x.NavigateToAsync(ApplicationPages.Essential.HomePage, It.IsAny<bool>()), Times.Never);
    }
    #endregion

    // #region Register()
    // [Theory]
    // [InlineData("someEmail@email.com", "someUsername", "0123456789")]
    // public async void Register_WhenSuccess(string email, string username, string mobileNo)
    // {
    //     var model = new CustomerRegisterDto
    //     {
    //         Person =
    //         {
    //             FirstName = username,
    //             LastName = username,
    //             NationalityCode = "EGP"
    //         },
    //         User =
    //         {
    //             Email = email,
    //             Username = username,
    //             Password = "somePassword",
    //             //PasswordConfirmation = "somePassword",
    //             MobileNo = mobileNo
    //         },
    //         Customer =
    //         {
    //             CountryCode = "EGP"
    //         }
    //     };
    //
    //     using var helper = new HostTestHelper();
    //     var service = helper.GetService<IUserService>();
    //
    //     await service.Register(model);
    //
    //     helper.DeviceMock.Verify(x =>
    //         x.NavigateToAsync(ApplicationPages.SplashPage, It.IsAny<bool>()), Times.AtLeastOnce);
    // }
    //
    // [Theory]
    // [InlineData("username@email.com", "username", "0123456")]
    // [InlineData("username1@email.com1", "username", "0123456")]
    // [InlineData("username@email.com", "username1", "0123456")]
    // public async void Register_WhenBusinessNotAllow_ExistingUser(string email, string username, string mobileNo)
    // {
    //     var httpClient = factory.WithWebHostBuilder(builder =>
    //     {
    //         builder.ConfigureServices(services =>
    //         {
    //             using var scope = services.BuildServiceProvider().CreateScope();
    //             var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
    //             var user = factory.GetUserDboWithEncryptedPassword("username", "password");
    //             db.Users.Add(user);
    //             user.Suspend("for testing");
    //             db.SaveChanges();
    //         });
    //     }).CreateClient();
    //
    //     using var helper = new TestHelper(nameof(RequestPasswordReset_WhenBusinessNotAllow_IsLockedUser));
    //     var service = helper.GetService<IHomeService>(httpClient);
    //
    //     var model = new CustomerRegisterDto
    //     {
    //         Person =
    //         {
    //             FirstName = username,
    //             LastName = username,
    //             NationalityCode = "EGP"
    //         },
    //         User =
    //         {
    //             Email = email,
    //             Username = username,
    //             Password = "somePassword",
    //             MobileNo = mobileNo
    //         },
    //         Customer =
    //         {
    //             CountryCode = "EGP"
    //         }
    //     };
    //
    //     await service.Register(model);
    //
    //     helper.DeviceMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
    //     helper.DeviceMock.Verify(x =>
    //         x.NavigateToAsync(ApplicationPages.SplashPage, It.IsAny<bool>()), Times.Never); ;
    // }
    // #endregion

    private HttpClient GetHttpClient(bool lockUser = false)
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                CoreTestHelper.SeedDatabase(db);
                
                if (lockUser)
                {
                    db.Users.First().Suspend("SomeReason");
                    db.SaveChanges();
                }
            });
        }).CreateClient();

        return httpClient;
    }
}