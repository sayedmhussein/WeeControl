using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Interfaces;
using WeeControl.Frontend.ApplicationService;
using WeeControl.Frontend.ApplicationService.Essential.ViewModels;
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
        using var helper = new TestHelper<UserLegacyViewModel>(factory.CreateClient());
        helper.ViewModel.FirstName = username;
        helper.ViewModel.LastName = username;
        helper.ViewModel.Email = email;
        helper.ViewModel.Username = username;
        helper.ViewModel.Password = "somePassword";
        helper.ViewModel.MobileNo = mobileNo;
        helper.ViewModel.Territory = "TST";
        helper.ViewModel.Nationality = "EGP";
        
        await helper.ViewModel.RegisterAsync();

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
        
        using var helper = new TestHelper<UserLegacyViewModel>(httpClient);

        helper.ViewModel.FirstName = username;
        helper.ViewModel.LastName = username;
        helper.ViewModel.Email = email;
        helper.ViewModel.Username = username;
        helper.ViewModel.Password = "somePassword";
        helper.ViewModel.MobileNo = mobileNo;
        helper.ViewModel.Territory = "TST";
        helper.ViewModel.Nationality = "EGP";
        
        await helper.ViewModel.RegisterAsync();
            
        helper.DeviceMock.AlertMock.Verify(x => x.DisplayAlert(It.IsAny<string>()));
        helper.DeviceMock.NavigationMock.Verify(x => 
            x.NavigateToAsync(Pages.Shared.IndexPage, It.IsAny<bool>()), Times.Never);;
    }
}