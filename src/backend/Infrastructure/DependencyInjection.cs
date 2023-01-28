using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.ApiApp.Infrastructure.Notifications;

namespace WeeControl.ApiApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEmailNotificationService>(p => new EmailService(configuration.GetConnectionString("EmailProvider")));

        return services;
    }
}