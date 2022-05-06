using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.Protected;
using WeeControl.Common.SharedKernel.DataTransferObjects.Authorization.User;
using WeeControl.Common.SharedKernel.RequestsResponses;
using WeeControl.Frontend.ServiceLibrary.BoundedContexts.Authorization;
using WeeControl.Frontend.ServiceLibrary.Interfaces;

namespace WeeControl.Common.ServiceLibrary.Test.CredentialsOperations;

public class LoginTests : IDisposable
{
    private Mock<IUserDevice> userDeviceMock;
    private Mock<IUserCommunication> userCommunicationMockMock;
    private Mock<IUserStorage> userStorageMockMock;
    
    public LoginTests()
    {
        userDeviceMock = new Mock<IUserDevice>();
        userDeviceMock.SetupAllProperties();
        userDeviceMock.Setup(x => x.DeviceId).Returns(nameof(LoginTests));

        userCommunicationMockMock = new Mock<IUserCommunication>();
        userCommunicationMockMock.SetupAllProperties();
        userCommunicationMockMock.Setup(x => x.ServerBaseAddress).Returns("http://localhost.com/");

        userStorageMockMock = new Mock<IUserStorage>();
        userStorageMockMock.SetupAllProperties();
    }
    
    public void Dispose()
    {
        userDeviceMock = null;
        userCommunicationMockMock = null;
        userStorageMockMock = null;
    }

    [Fact]
    public async void WhenValidLoginCredentials_TokenIsSaved()
    {
        userCommunicationMockMock.Setup(x => x.HttpClient)
            .Returns(GetHttpClientForTesting(
                HttpStatusCode.OK, 
                RequestDto.BuildHttpContentAsJson(
                new ResponseDto<TokenDto>(new TokenDto("token", "fullname", "")))));
        
        var response = 
            await new UserOperation(userDeviceMock.Object, userCommunicationMockMock.Object, userStorageMockMock.Object)
            .LoginAsync(new LoginDto() {UsernameOrEmail = "admin", Password = "admin"});
        
        Assert.True(response.IsSuccess);
    }

    private HttpClient GetHttpClientForTesting(HttpStatusCode statusCode, HttpContent content)
    {
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        var response = new HttpResponseMessage
        {
            StatusCode = statusCode, 
            Content = content
        };
        
        httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(response);
        
         return new HttpClient(httpMessageHandlerMock.Object);
    }

    
}