using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Presentations.FunctionalService.Enums;
using WeeControl.Presentations.FunctionalService.Interfaces;
using WeeControl.SharedKernel.Services;
using Xunit;

namespace WeeControl.WebApi.Test.Functional.Controllers.Essentials.UserOperation;

public class GetTokenTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IDisposable
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
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .GetTokenAsync();
            
        Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
        Assert.NotEmpty(token2);
        
        return token2;
    }
    #endregion
        
    private readonly CustomWebApplicationFactory<Startup> factory;
    private HttpClient client;
    private readonly (string Email, string Username, string Password, string Device) user = (Email: "test@test.test", Username: "test", Password: "test", Device: typeof(GetTokenTests).Namespace);

    public GetTokenTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
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
        client = null;
    }
        
    [Fact]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode()
    {
        //var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(GetTokenTests).Namespace);
        var token = await LoginTests.LoginAsync(client, user.Email, user.Password, typeof(GetTokenTests).Namespace);
        mocks.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

        var response = 
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .GetTokenAsync();
            
        Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
    }

    [Fact]
    public async void WhenSendingValidRequest_HttpResponseIsSuccessCode2()
    {
        var token = await GetRefreshedTokenAsync(client, user.Username, user.Password, typeof(GetTokenTests).Namespace);
            
        Assert.NotEmpty(token);
    }
        
    [Fact]
    public async void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized()
    {
        //var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(GetTokenTests).Namespace);

        var response = 
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .GetTokenAsync();

        Assert.Equal(HttpStatusCode.Unauthorized, response.HttpStatusCode);
    }

    [Fact]
    public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
    {
        //When different device...
        //var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, "Some Other Device");
        var token = await LoginTests.LoginAsync(client, user.Username, user.Password, typeof(GetTokenTests).Namespace);
        mocks.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

        var response = 
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .GetTokenAsync();

        Assert.Equal(HttpStatusCode.Forbidden, response.HttpStatusCode);
    }
}