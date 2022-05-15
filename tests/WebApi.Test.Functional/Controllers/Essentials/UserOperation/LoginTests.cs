using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Presentations.FunctionalService.Enums;
using WeeControl.Presentations.FunctionalService.Interfaces;
using WeeControl.SharedKernel.Essential.RequestDTOs;
using WeeControl.SharedKernel.Interfaces;
using WeeControl.SharedKernel.Services;
using Xunit;

namespace WeeControl.WebApi.Test.Functional.Controllers.Essentials.UserOperation;

public class LoginTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    #region static
    public static async Task<string> LoginAsync(HttpClient client, string username, string password, string device)
    {
        var token = string.Empty;

        var mocks = ApplicationMocks.GetEssentialMock(client, device);
        mocks.Setup(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()))
            .Callback((UserDataEnum en, string tkn) => token = tkn);
            
        var response = 
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .LoginAsync(new LoginDto(username, password));
            
        Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
        Assert.NotEmpty(token);
            
        return token;
    }

    private static Task<IResponseDto> LoginDebugAsync(HttpClient client, string username, string password, string device)
    {
        var mocks = ApplicationMocks.GetEssentialMock(client, device);

        var response = new Presentations.FunctionalService.EssentialContext.UserOperation(
                mocks.Object, 
                new Mock<IDisplayAlert>().Object)
            .LoginAsync(new LoginDto(username, password));

        return response;
    }
    #endregion
        
    private readonly CustomWebApplicationFactory<Startup> factory;

    public LoginTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("", "password")]
    [InlineData("username", "")]
    public async void WhenSendingInvalidRequest_HttpResponseIsBadRequest(string username, string password)
    {
        var client = factory.CreateClient();
            
        var response = await LoginDebugAsync(client, username, password, nameof(WhenSendingInvalidRequest_HttpResponseIsBadRequest));
            
        Assert.Equal(HttpStatusCode.BadRequest, response.HttpStatusCode);
    }
        
    [Fact]
    public async void WhenUserNotExist_HttpResponseIsNotFound()
    {
        var client = factory.CreateClient();
            
        var response = await LoginDebugAsync(client, "unknown", "unknown", nameof(WhenSendingInvalidRequest_HttpResponseIsBadRequest));
            
        Assert.Equal(HttpStatusCode.NotFound, response.HttpStatusCode);
    }

    [Fact]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
    {
        var mocks = ApplicationMocks.GetEssentialMock(factory.CreateClient(), typeof(LoginTests).Namespace);

        var response = 
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .LoginAsync(new LoginDto("admin", "admin"));
            
        Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
        Assert.True(response.IsSuccess);
        mocks.Verify(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()));
    }

    [Fact]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode2()
    {
        var user = (Email: "test@test.test", Username: "test", Password: "test", Device: typeof(LoginTests).Namespace);
        var client = factory.WithWebHostBuilder(builder =>
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
            
        var token = await LoginAsync(client, user.Username, user.Password, user.Device);
            
        Assert.NotEmpty(token);
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