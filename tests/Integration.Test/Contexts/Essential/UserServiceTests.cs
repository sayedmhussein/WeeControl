using Microsoft.Extensions.DependencyInjection;
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
    
    [Fact]
    public async void ChangeMyPassword_WhenUserIsLocked2()
    {
        using var helper = new HostTestHelper();
        var service = helper.GetService<IUserService>(factory.CreateCustomClient(factory, db =>
        {
            db.Users.First().Suspend("SomeReason");
            db.SaveChanges();
        }));
        
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