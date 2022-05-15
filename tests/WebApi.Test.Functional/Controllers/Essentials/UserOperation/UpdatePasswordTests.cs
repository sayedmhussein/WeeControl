using System.Net;
using Moq;
using WeeControl.Presentations.FunctionalService.Enums;
using WeeControl.Presentations.FunctionalService.Interfaces;
using WeeControl.SharedKernel.Essential.RequestDTOs;
using Xunit;

namespace WeeControl.WebApi.Test.Functional.Controllers.Essentials.UserOperation;

public class UpdatePasswordTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public UpdatePasswordTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void WhenUserAuthenticatedAndSendRequest_ReturnOk()
    {
        var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(UpdatePasswordTests).Namespace);
        var token = await GetTokenTests.GetRefreshedTokenAsync(client, "admin", "admin", typeof(LogoutTests).Namespace);
        mocks.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

        var dto = new PasswordSetForgottenDto()
            {OldPassword = "admin", NewPassword = "NewPassword", ConfirmNewPassword = "NewPassword"};
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
        var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(UpdatePasswordTests).Namespace);

        var response = 
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .UpdatePasswordAsync(new PasswordSetForgottenDto() { NewPassword = "NewPassword", ConfirmNewPassword = "NewPassword"});
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.HttpStatusCode);
    }
}