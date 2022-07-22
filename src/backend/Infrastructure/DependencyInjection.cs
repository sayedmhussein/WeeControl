using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Domain.Interfaces;
using WeeControl.Infrastructure.Notifications;

namespace WeeControl.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEmailNotificationService>(p => new EmailService(configuration.GetConnectionString("EmailProvider")));

        return services;
    }
}