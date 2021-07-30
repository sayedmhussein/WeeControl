using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.Server.Domain.Interfaces;
using WeeControl.Server.Infrastructure.Notifications;

namespace WeeControl.Server.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEmailNotificationService>(provider => new EmailService(configuration.GetConnectionString("EmailProvider")));

            return services;
        }
    }
}
