using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Domain.Interfaces;
using WeeControl.Infrastructure.Notifications;
using WeeControl.Persistence;

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
    
    
}