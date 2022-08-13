using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Interfaces;
using WeeControl.Frontend.ApplicationService;
using WeeControl.Frontend.ApplicationService.Customer.Models;
using WeeControl.WebApi;
using Xunit;

namespace WeeControl.User.UserApplication.Test.Integration.ViewModels.User;

public class RegisterTests : IClassFixture<CustomWebApplicationFactory<Startup>>
{
    private readonly CustomWebApplicationFactory<Startup> factory;

    public RegisterTests(CustomWebApplicationFactory<Startup> factory)
    {
        this.factory = factory;
    }
    
    [Theory]
    [InlineData("someEmail@email.com", "someUsername", "0123456789")]
    public async void WhenSuccess(string email, string username, string mobileNo)
    {
        var model = new UserRegisterModel();
        model.FirstName = username;
        model.LastName = username;
        model.Email = email;
        model.Username = username;
        model.Password = "somePassword";
        model.MobileNo = mobileNo;
        model.TerritoryId = "TST";
        model.Nationality = "EGP";
        
        using var helper = new TestHelper<CustomerViewModel>(factory.CreateClient());
        
        await helper.ViewModel.RegisterAsync(model);

        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Once);
    }
    
    [Theory]
    [InlineData("username@email.com", "username", "0123456")]
    [InlineData("username1@email.com1", "username", "0123456")]
    [InlineData("username@email.com", "username1", "0123456")]
    public async void WhenBusinessNotAllow_ExistingUser(string email, string username, string mobileNo)
    {
        var httpClient = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<IEssentialDbContext>();
                var user = TestHelper<object>.GetUserDboWithEncryptedPassword("username", "password");
                db.Users.Add(user);
                user.Suspend("for testing");
                db.SaveChanges();
            });
        }).CreateClient();
        
        using var helper = new TestHelper<CustomerViewModel>(httpClient);

        var model = new UserRegisterModel();
        model.FirstName = username;
        model.LastName = username;
        model.Email = email;
        model.Username = username;
        model.Password = "somePassword";
        model.MobileNo = mobileNo;
        model.TerritoryId = "TST";
        model.Nationality = "EGP";
        
        await helper.ViewModel.RegisterAsync(model);
            
        helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);;
    }
}