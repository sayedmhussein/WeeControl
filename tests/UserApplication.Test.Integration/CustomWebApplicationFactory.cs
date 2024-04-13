using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using WeeControl.ApiApp.Infrastructure.Notifications;
using WeeControl.ApiApp.Persistence;
using WeeControl.Frontend.AppService.GuiInterfaces.Authorization;
using WeeControl.Frontend.Service.UnitTest;

namespace WeeControl.User.UserApplication.Test.Integration;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddPersistenceAsInMemory();

            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(EmailService));

            services.Remove(descriptor);

            services.AddSingleton<IEmailNotificationService>(p => new Mock<IEmailNotificationService>().Object);

        });
    }

    public UserDbo GetUserDboWithEncryptedPassword(string username, string password, string territory = "TST")
    {
        return new UserDbo(new UserEntity()
        {
            Username = username,
            Password = new PasswordSecurity().Hash(password),
            MobileNo = "012345667",
            Email = (username + "@email.com").ToLower()
        });
    }

    public async Task Authorize(TestHelper testHelper, string username, string password)
    {
        var service = testHelper.GetService<IAuthorizationService>();

        await service.Login(username, password);
        await service.UpdateToken("0000");
    }
}