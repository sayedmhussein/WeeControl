using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Application.Common.Interfaces;
using MySystem.Infrastructure.NotificationServices;

namespace MySystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEmailNotificationService>(provider => new EmailService(configuration["Notification:EmailConfigurationString"]));

            return services;
        }
    }
}
