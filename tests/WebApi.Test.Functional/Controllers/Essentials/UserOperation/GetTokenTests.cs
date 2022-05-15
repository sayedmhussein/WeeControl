using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using WeeControl.Frontend.FunctionalService.Enums;
using WeeControl.Frontend.FunctionalService.Interfaces;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.Test.WebApi.Test.Functional.Controllers.Essentials.UserOperation;

public class GetTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    #region static
    public static async Task<string> GetRefreshedTokenAsync(HttpClient client, string username, string password,
        string device)
    {
        var token1 = await LoginTests.LoginAsync(client, username, password, device);
        var token2 = string.Empty;
            
        var mocks = ApplicationMocks.GetEssentialMock(client, device);
        mocks.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token1);
        mocks.Setup(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()))
            .Callback((UserDataEnum en, string tkn) => token2 = tkn);

        var response = 
            await new Frontend.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .GetTokenAsync();
            
        Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
        Assert.NotEmpty(token2);
        
        return token2;
    }
    #endregion
        
    private readonly CustomWebApplicationFactory<Startup> factory;

    public GetTokenTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }
        
    [Fact]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
    {
        var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(GetTokenTests).Namespace);
        var token = await LoginTests.LoginAsync(client, "admin", "admin", typeof(GetTokenTests).Namespace);
        mocks.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

        var response = 
            await new Frontend.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .GetTokenAsync();
            
        Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
    }

    [Fact]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode2()
    {
        var token = await GetRefreshedTokenAsync(factory.CreateClient(), "admin", "admin", typeof(GetTokenTests).Namespace);
            
        Assert.NotEmpty(token);
    }
        
    [Fact]
    public async void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized()
    {
        var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(GetTokenTests).Namespace);

        var response = 
            await new Frontend.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .GetTokenAsync();

        Assert.Equal(HttpStatusCode.Unauthorized, response.HttpStatusCode);
    }

    [Fact]
    public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
    {
        //When different device...
        var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, "Some Other Device");
        var token = await LoginTests.LoginAsync(client, "admin", "admin", typeof(GetTokenTests).Namespace);
        mocks.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

        var response = 
            await new Frontend.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .GetTokenAsync();

        Assert.Equal(HttpStatusCode.Forbidden, response.HttpStatusCode);
    }
}