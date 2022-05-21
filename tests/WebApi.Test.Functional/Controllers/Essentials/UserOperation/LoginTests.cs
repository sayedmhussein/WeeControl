using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Presentations.ServiceLibrary.Enums;
using WeeControl.SharedKernel.Essential.DataTransferObjects;
using WeeControl.SharedKernel.Services;
using Xunit;

namespace WeeControl.WebApi.Test.Functional.Controllers.Essentials.UserOperation;

public class LoginTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
{
    #region static
    public static async Task<string> LoginAsync(HttpClient client, string username, string password, string device)
    {
        var token = string.Empty;
    
        var mocks = new DeviceServiceMock(device);
        mocks.StorageMock.Setup(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()))
            .Callback((UserDataEnum en, string tkn) => token = tkn);
        
        await new Presentations.ServiceLibrary.EssentialContext.UserOperation(
                mocks.GetObject(client))
            .LoginAsync(new LoginDtoV1(username, password));
            
        Assert.NotEmpty(token);
            
        return token;
    }
    #endregion
        
    private readonly CustomWebApplicationFactory<Startup> factory;
    private DeviceServiceMock deviceMock;
    private readonly (string Email, string Username, string Password, string Device) user = 
        (Email: "test@test.test", Username: "test", Password: "test", Device: typeof(LoginTests).Namespace);

    public LoginTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
        deviceMock = new DeviceServiceMock(nameof(LoginTests));
    }

    public void Dispose()
    {
        deviceMock = null;
    }

    [Theory]
    [InlineData("", "", AlertEnum.DeveloperInvalidUserInput)]
    [InlineData("", "password", AlertEnum.DeveloperInvalidUserInput)]
    [InlineData("username", "", AlertEnum.DeveloperInvalidUserInput)]
    [InlineData("usernotexist", "usernotexist", AlertEnum.InvalidUsernameOrPassword)]
    public async void TestsForFailedScenarios(string username, string password, AlertEnum alertEnum)
    {
        var client = factory.CreateClient();
        
        await new Presentations.ServiceLibrary.EssentialContext.UserOperation(deviceMock.GetObject(client))
            .LoginAsync(new LoginDtoV1(username, password));
            
        deviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(alertEnum), Times.Once);
        
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(PagesEnum.Home, It.IsAny<bool>()), Times.Never);
    }
        
    [Fact]
    public async void WhenUserIsLocked()
    {
        var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    using var scope = services.BuildServiceProvider().CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                    var usr = UserDbo.Create(user.Email, user.Username, new PasswordSecurity().Hash(user.Password));
                    usr.Suspend("For test purposes only.");
                    db.Users.Add(usr);
                    db.SaveChanges();
                });
            })
            .CreateClient();
        
        await new Presentations.ServiceLibrary.EssentialContext.UserOperation(deviceMock.GetObject(client))
            .LoginAsync(new LoginDtoV1(user.Username, user.Password));
        
        deviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(PagesEnum.Home, It.IsAny<bool>()), Times.Never);
        
        deviceMock.AlertMock.Verify(x => 
            x.DisplayAlert(AlertEnum.AccountIsLocked), Times.Once);
    }

    [Fact]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
    {
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
        
        var mocks = new DeviceServiceMock(typeof(LoginTests).Namespace);
        
        await new Presentations.ServiceLibrary.EssentialContext.UserOperation(mocks.GetObject(client))
            .LoginAsync(new LoginDtoV1(user.Username, user.Password));

        mocks.StorageMock.Verify(x => x.
            SaveAsync(UserDataEnum.Token, It.IsAny<string>()));
        mocks.NavigationMock.Verify(x => x.NavigateToAsync(PagesEnum.Home, It.IsAny<bool>()), Times.Once);
    }
    
    [Fact]
    public async void TestForStsticFunction()
    {
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

        var token = await LoginAsync(client, user.Username, user.Password, "device");
        
        Assert.NotEmpty(token);
    }

    [Obsolete("Unused function!!!")]
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