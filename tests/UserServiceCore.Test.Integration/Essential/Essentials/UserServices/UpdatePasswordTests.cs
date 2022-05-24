using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Services;
using WeeControl.User.UserServiceCore.Enums;
using WeeControl.User.UserServiceCore.Services;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.ServiceCore.Test.Integration.Essential.Essentials.UserServices;

public class UpdatePasswordTests : IClassFixture<CustomWebApplicationFactory<Startup>>, System.IDisposable
{
    private HttpClient client;
    private DeviceServiceMock deviceMock;
    private readonly (string Email, string Username, string Password, string Device) user = (Email: "test@test.test", Username: "test", Password: "test", Device: typeof(UpdatePasswordTests).Namespace);

    public UpdatePasswordTests(CustomWebApplicationFactory<Startup> factory)
    {
        client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    using var scope = services.BuildServiceProvider().CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                    db.Users.Add(UserDbo.Create(user.Email, user.Username, new PasswordSecurity().Hash(user.Password)));
                    db.SaveChanges();
                });
            })
            .CreateClient();

        deviceMock = new DeviceServiceMock(nameof(UpdatePasswordTests));
    }

    public void Dispose()
    {
        client = null;
        deviceMock = null;
    }

    [Fact]
    public async void WhenUserAuthenticatedAndSendRequest_ReturnOk()
    {
        var token = await GetTokenTests.GetRefreshedTokenAsync(client, user.Username, user.Password, nameof(UpdatePasswordTests));
        deviceMock.StorageMock.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

        var dto = new MeForgotPasswordDtoV1()
            {OldPassword = user.Password, NewPassword = "NewPassword", ConfirmNewPassword = "NewPassword"};
        
        await new UserService(
                    deviceMock.GetObject(client))
                .UpdatePasswordAsync(dto);
        
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(PagesEnum.Home, It.IsAny<bool>()), Times.Once);
        
        deviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(AlertEnum.PasswordUpdatedSuccessfully), Times.Once);
    }
    
    [Fact]
    public async void WhenOldPasswordIsWrong()
    {
        var token = await GetTokenTests.GetRefreshedTokenAsync(client, user.Username, user.Password, nameof(UpdatePasswordTests));
        deviceMock.StorageMock.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

        var dto = new MeForgotPasswordDtoV1()
            {OldPassword = user.Password + "bla", NewPassword = "NewPassword", ConfirmNewPassword = "NewPassword"};
        
        await new UserService(
                deviceMock.GetObject(client))
            .UpdatePasswordAsync(dto);
        
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(PagesEnum.Home, It.IsAny<bool>()), Times.Never);
        
        deviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(AlertEnum.InvalidPassword), Times.Once);
    }
}