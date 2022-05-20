using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Presentations.FunctionalService.Enums;
using WeeControl.Presentations.FunctionalService.Interfaces;
using WeeControl.SharedKernel.Services;
using Xunit;

namespace WeeControl.WebApi.Test.Functional.Controllers.Essentials.UserOperation;

public class LogoutTests : IClassFixture<CustomWebApplicationFactory<Startup>>, System.IDisposable
{
    //private readonly CustomWebApplicationFactory<Startup> factory;
    private HttpClient client;
    private readonly (string Email, string Username, string Password, string Device) user = (Email: "test@test.test", Username: "test", Password: "test", Device: typeof(LogoutTests).Namespace);

    public LogoutTests(CustomWebApplicationFactory<Startup> factory)
    {
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
            
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(LogoutTests).Namespace);
        var token = await GetTokenTests.GetRefreshedTokenAsync(client, user.Username, user.Password, typeof(LogoutTests).Namespace);
        mocks.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);

        var response = 
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .LogoutAsync();
            
        Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
    }

    [Fact]
    public async void WhenUnAuthenticatedUser_HttpResponseIsUnauthorized()
    {
        var mocks = ApplicationMocks.GetEssentialMock(client, nameof(WhenSendingValidRequest_HttpResponseIsSuccessCode));

        var response = 
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .LogoutAsync();
            
        Assert.Equal(HttpStatusCode.Unauthorized, response.HttpStatusCode);
    }

    [Fact]
    public async void WhenAuthenticatedButNotAuthorized_HttpResponseIsForbidden()
    {
        //var client = factory.CreateClient();
            
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(LogoutTests).Namespace);
        var token = await GetTokenTests.GetRefreshedTokenAsync(client, user.Email, user.Password, typeof(LogoutTests).Namespace);
        mocks.Setup(x => x.GetAsync(UserDataEnum.Token)).ReturnsAsync(token);
            
        var response1 = 
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .LogoutAsync();
            
        var response2 = 
            await new Presentations.FunctionalService.EssentialContext.UserOperation(
                    mocks.Object, 
                    new Mock<IDisplayAlert>().Object)
                .LogoutAsync();
        Assert.Equal(HttpStatusCode.Forbidden, response2.HttpStatusCode);
    }
}