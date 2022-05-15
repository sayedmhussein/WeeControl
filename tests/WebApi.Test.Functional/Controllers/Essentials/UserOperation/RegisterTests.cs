using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Frontend.FunctionalService.Enums;
using WeeControl.Frontend.FunctionalService.Interfaces;
using WeeControl.SharedKernel.Essential.RequestDTOs;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.Test.WebApi.Test.Functional.Controllers.Essentials.UserOperation;

public class RegisterTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public RegisterTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async void WhenNewUserRegisterWithValidData_ReturnSuccess()
    {
        var mocks = ApplicationMocks.GetEssentialMock(factory.CreateClient(), typeof(RegisterTests).Namespace);

        var response = 
            await new Frontend.FunctionalService.EssentialContext.UserOperation(
                mocks.Object, 
                new Mock<IDisplayAlert>().Object).RegisterAsync(RegisterDto.Create("email@email.com", "username", "password"));
            

        Assert.Equal(HttpStatusCode.OK, response.HttpStatusCode);
        Assert.True(response.IsSuccess);
        mocks.Verify(x => x.SaveAsync(UserDataEnum.Token, It.IsAny<string>()));
    }
        
    [Theory]
    [InlineData("", "", "")]
    [InlineData("", "username", "password")]
    [InlineData("email@email.com", "", "password")]
    [InlineData("email@email.com", "username", "")]
    [InlineData("emil", "username", "password")]
    [InlineData("email@email.com", "us", "password")]
    [InlineData("email@email.com", "username", "pas")]
    public async void WhenNewUserRegisterWithInValidData_ReturnBadRequest(string email, string username, string password)
    {
        var mocks = ApplicationMocks.GetEssentialMock(factory.CreateClient(), typeof(RegisterTests).Namespace);

        var response = 
            await new Frontend.FunctionalService.EssentialContext.UserOperation(
                mocks.Object, 
                new Mock<IDisplayAlert>().Object).RegisterAsync(RegisterDto.Create(email, username, password));
            

        Assert.Equal(HttpStatusCode.BadRequest, response.HttpStatusCode);
    }
        
    [Fact]
    public async void WhenNewUserRegisterExistingEmail_ReturnConflict()
    {
        var user = (Email: "exist@exist.com", Username: "exist", Password: "password", Device: typeof(LoginTests).Namespace);
        var client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    using var scope = services.BuildServiceProvider().CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                    db.Users.Add(UserDbo.Create(user.Email, user.Username, user.Password));
                    db.SaveChanges();
                });
            })
            .CreateClient();
        var mocks = ApplicationMocks.GetEssentialMock(client, typeof(RegisterTests).Namespace);

        var responseSameEmail = 
            await new Frontend.FunctionalService.EssentialContext.UserOperation(
                mocks.Object, 
                new Mock<IDisplayAlert>().Object).RegisterAsync(RegisterDto.Create(user.Email, "username", "password"));
            

        Assert.Equal(HttpStatusCode.Conflict, responseSameEmail.HttpStatusCode);
            
        var responseSameUsername = 
            await new Frontend.FunctionalService.EssentialContext.UserOperation(
                mocks.Object, 
                new Mock<IDisplayAlert>().Object).RegisterAsync(RegisterDto.Create("someemail@email.com", user.Username, "password"));
        Assert.Equal(HttpStatusCode.Conflict, responseSameUsername.HttpStatusCode);
    }
}