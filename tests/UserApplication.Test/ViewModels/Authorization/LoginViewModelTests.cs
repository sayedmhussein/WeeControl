using System.Net;
using System.Text;
using Moq.Protected;
using Newtonsoft.Json;
using WeeControl.SharedKernel.DataTransferObjects.Authentication;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.User.UserApplication.ViewModels.Authentication;

namespace WeeControl.User.UserApplication.Test.ViewModels.Authorization;

public class LoginViewModelTests : IDisposable
{
    private DeviceServiceMock mock;
    
    public LoginViewModelTests()
    {
        mock = new DeviceServiceMock(nameof(LoginViewModelTests));
    }

    public void Dispose()
    {
        mock = null;
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "password")]
    [InlineData("username", "")]
    [InlineData("    ", "password")]
    [InlineData("username", "    ")]
    [InlineData("   ", "    ")]
    public async void WhenEmptyProperties_DisplayAlertOnly(string username, string password)
    {
        var client = GetHttpClientForTesting_(HttpStatusCode.OK, 
            new ResponseDto<TokenDtoV1>(TokenDtoV1.Create("token", "name", "url")));
        var vm = new LoginViewModel(mock.GetObject(client))
        {
            UsernameOrEmail = username,
            Password = password
        };
        
        await vm.LoginAsync();
        
        mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.Once);
        mock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync(It.IsAny<string>()), Times.Never);
        mock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index,true), Times.Never);
    }
    
    [Fact]
    public async void WhenServerReturnsOkWithValidDto()
    {
        // var client = GetHttpClientForTesting_(HttpStatusCode.OK, 
        //     TokenDtoV1.Create("token", "name", "url"));
        var client = GetHttpClientForTesting_(HttpStatusCode.OK, 
            new ResponseDto<TokenDtoV1>(TokenDtoV1.Create("token", "name", "url")));
        var vm = new LoginViewModel(mock.GetObject(client))
        {
            UsernameOrEmail = "username",
            Password = "password"
        };
        
        await vm.LoginAsync();
        
        mock.AlertMock.Verify(x => 
            x.DisplayAlert(It.IsAny<string>()), Times.AtMost(1));
        mock.SecurityMock.Verify(x => 
            x.UpdateTokenAsync("token"), Times.AtLeastOnce);
        mock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Home.Index,true), Times.Once);
    }
    
    private HttpClient GetHttpClientForTesting_<T>(HttpStatusCode statusCode, T dto)
    {
        var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
        return GetHttpClientForTesting(statusCode, content);
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