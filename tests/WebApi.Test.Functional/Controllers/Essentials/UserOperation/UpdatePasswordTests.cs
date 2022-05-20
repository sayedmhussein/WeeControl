using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Presentations.FunctionalService.Enums;
using WeeControl.Presentations.FunctionalService.Interfaces;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Services;
using Xunit;

namespace WeeControl.WebApi.Test.Functional.Controllers.Essentials.UserOperation;

public class UpdatePasswordTests : IClassFixture<CustomWebApplicationFactory<Startup>>, System.IDisposable
{
    private HttpClient client;
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
    }

    public void Dispose()
    {
        
    }

    [Fact]
    public async void WhenUserAuthenticatedAndSendRequest_ReturnOk()
    {
        //var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(UpdatePasswordTests).Namespace);
        var token = await GetTokenTests.GetRefreshedTokenAsync(client, user.Username, user.Password, typeof(LogoutTests).Namespace);
        mocks.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

        var dto = new PasswordSetForgottenDto()
            {OldPassword = user.Password, NewPassword = "NewPassword", ConfirmNewPassword = "NewPassword"};
        var response = 
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .UpdatePasswordAsync(dto);
        
        Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
    }
    
    [Fact]
    public async void WhenUserUnAuthenticated_ReturnUnAuthorized()
    {
        //var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(UpdatePasswordTests).Namespace);

        var response = 
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .UpdatePasswordAsync(new PasswordSetForgottenDto() { NewPassword = "NewPassword", ConfirmNewPassword = "NewPassword"});
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.HttpStatusCode);
    }
}