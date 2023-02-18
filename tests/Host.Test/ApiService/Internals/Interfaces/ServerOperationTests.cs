using System.Net;
using WeeControl.Core.DataTransferObject.Contexts.User;
using WeeControl.Host.WebApiService.Internals.Interfaces;

namespace WeeControl.Host.Test.ApiService.Internals.Interfaces;

public class ServerOperationTests
{
    #region GetResponseMessageTests

    [Fact]
    public async void GetResponseMessage_Test()
    {
        using var testingHelper = new TestingServiceHelper();
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
        using var testingHelper = new TestingServiceHelper();
        testingHelper.StorageMock.Setup(x => x.GetKeyValue(IDeviceSecurity.TokenKeyName)).ReturnsAsync("Something");

        var service = testingHelper.GetService<IServerOperation>(HttpStatusCode.OK, TokenResponseDto.Create("Token"));
        
        Assert.True(await service.RefreshToken());
    }
    
    [Fact]
    public async void RefreshToken_WhenNoTokenInDevice_ReturnFalse()
    {
        using var testingHelper = new TestingServiceHelper();
        testingHelper.StorageMock.Setup(x => x.GetKeyValue(IDeviceSecurity.TokenKeyName)).ReturnsAsync(string.Empty);

        var service = testingHelper.GetService<IServerOperation>(HttpStatusCode.OK, TokenResponseDto.Create("Token"));

        Assert.False(await service.RefreshToken());
    }

    [Fact]
    public async void RefreshToken_WhenServerReject_ReturnFalseAndTokenGetRemoved()
    {
        using var testingHelper = new TestingServiceHelper();

        var service = testingHelper.GetService<IServerOperation>(HttpStatusCode.Forbidden);
        
        Assert.False(await service.RefreshToken());
    }
    #endregion
    
}