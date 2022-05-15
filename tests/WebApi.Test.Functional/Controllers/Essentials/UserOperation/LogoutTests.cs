using System.Net;
using Moq;
using WeeControl.Frontend.FunctionalService.Enums;
using WeeControl.Frontend.FunctionalService.Interfaces;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.Test.WebApi.Test.Functional.Controllers.Essentials.UserOperation;

public class LogoutTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public LogoutTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
    {
        var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(LogoutTests).Namespace);
        var token = await GetTokenTests.GetRefreshedTokenAsync(client, "admin", "admin", typeof(LogoutTests).Namespace);
        mocks.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

        var response = 
            await new Frontend.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .LogoutAsync();
            
        Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
    }

    [Fact]
    public async void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized()
    {
        var mocks = ApplicationMocks.GetEssentialMock(factory.CreateClient(), nameof(WhenSendingValidRequest_HttpResponseIsSuccessCode));

        var response = 
            await new Frontend.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .LogoutAsync();
            
        Assert.Equal(HttpStatusCode.Unauthorized, response.HttpStatusCode);
    }

    [Fact]
    public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
    {
        var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(LogoutTests).Namespace);
        var token = await GetTokenTests.GetRefreshedTokenAsync(client, "admin", "admin", typeof(LogoutTests).Namespace);
        mocks.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);
            
        var response1 = 
            await new Frontend.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .LogoutAsync();
            
        var response2 = 
            await new Frontend.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .LogoutAsync();
        Assert.Equal(HttpStatusCode.Forbidden, response2.HttpStatusCode);
    }
}