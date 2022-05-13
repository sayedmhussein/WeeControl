using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Backend.Domain.Interfaces;
using WeeControl.Backend.Infrastructure.Notifications;

namespace WeeControl.Backend.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEmailNotificationService>(provider => new EmailService(configuration.GetConnectionString("EmailProvider")));

        return services;
    }
}