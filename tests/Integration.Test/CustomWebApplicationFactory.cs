using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.ApiApp.Infrastructure.Notifications;
using WeeControl.ApiApp.Persistence;
using WeeControl.Core.Application.Interfaces;

namespace WeeControl.Integration.Test;

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

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddSingleton<IEmailNotificationService>(p => 
                new Mock<IEmailNotificationService>().Object);

        });
    }
}