using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using WeeControl.ApiApp.Infrastructure.Notifications;
using WeeControl.ApiApp.Persistence;
using WeeControl.Core.Application.Interfaces;
using WeeControl.Core.Domain.Contexts.User;
using WeeControl.Core.SharedKernel.Services;
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
        return UserDbo.Create(Guid.Empty, (username + "@email.com").ToLower(), username, "0123456789", new PasswordSecurity().Hash(password));
    }

    public async Task Authorize(TestHelper testHelper, string username, string password)
    {
        var service = testHelper.GetService<IAuthorizationService>();

        await service.Login(username, password);
        await service.UpdateToken("0000");
    }
}