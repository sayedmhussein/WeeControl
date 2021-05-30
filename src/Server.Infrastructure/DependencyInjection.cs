using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Application.Common.Interfaces;
using MySystem.Infrastructure.NotificationService;
using MySystem.Infrastructure.SecurityService;

namespace MySystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtService>(provider => new JwtService(configuration["Jwt:Key"]));
            services.AddSingleton<IEmailNotificationService>(provider => new EmailService(configuration["Notification:EmailConfigurationString"]));

            return services;
        }
    }
}
