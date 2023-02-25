using System.Net;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.Test.ApiService.Internals.Interfaces;

public class ServerOperationTests
{
    #region GetResponseMessageTests

    [Fact]
    public async void GetResponseMessage_Test()
    {
        using var testingHelper = new HostTestHelper();
        var service = testingHelper.GetService<IServerOperation>(HttpStatusCode.OK, TokenResponseDto.Create("Token"));

        var response = await service
            .GetResponseMessage(HttpMethod.Get, new Version("1.0"), "API/Test");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion

    #region RefreshTokenTests
    
    [Fact]
    public async void RefreshToken_WhenServerAccept_ReturnTrueAndTokenGetUpdated()
    {
        using var testingHelper = new HostTestHelper();
        testingHelper.StorageMock.Setup(x => x.GetKeyValue(IDeviceSecurity.TokenKeyName)).ReturnsAsync("Something");

        var service = testingHelper.GetService<IServerOperation>(HttpStatusCode.OK, TokenResponseDto.Create("Token"));
        
        Assert.True(await service.RefreshToken());
    }
    
    [Fact]
    public async void RefreshToken_WhenNoTokenInDevice_ReturnFalse()
    {
        using var testingHelper = new HostTestHelper();
        testingHelper.StorageMock.Setup(x => x.GetKeyValue(IDeviceSecurity.TokenKeyName)).ReturnsAsync(string.Empty);

        var service = testingHelper.GetService<IServerOperation>(HttpStatusCode.OK, TokenResponseDto.Create("Token"));

        Assert.False(await service.RefreshToken());
    }

    [Fact]
    public async void RefreshToken_WhenServerReject_ReturnFalseAndTokenGetRemoved()
    {
        using var testingHelper = new HostTestHelper();

        var service = testingHelper.GetService<IServerOperation>(HttpStatusCode.Forbidden);
        
        Assert.False(await service.RefreshToken());
    }
    #endregion

    #region CoffeBrewingTests

    [Fact]
    public async void WhenServerRespond418_RefreshToken()
    {
        using var testingHelper = new HostTestHelper();
        testingHelper.StorageMock.Setup(x => x.GetKeyValue(It.IsAny<string>())).ReturnsAsync("Token");
        var service = testingHelper.GetService<IServerOperation>( new List<(HttpStatusCode statusCode, object? dto)>()
        {
            ((HttpStatusCode)418, null), 
            (HttpStatusCode.OK, TokenResponseDto.Create("Token")),
            (HttpStatusCode.OK, TokenResponseDto.Create("Token"))
        });

        var response = await service
            .GetResponseMessage(HttpMethod.Get, new Version("1.0"), "API/Test");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    #endregion
}