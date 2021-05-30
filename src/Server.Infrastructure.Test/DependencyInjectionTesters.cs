using System;
using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySystem.Infrastructure.SecurityService;
using MySystem.Application.Common.Interfaces;

namespace MySystem.Infrastructure.Test
{
    public class DependencyInjectionTesters
    {
        [Fact]
        public void WhenAddingInfrastructure_JwtServiceObjectMustNotBeNull()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x["Jwt:Key"]).Returns("MySystem.Api.UnitTest.Controllers.V1.CredentialsController");

            var services = new ServiceCollection();
            services.AddInfrastructure(configMock.Object);
            var provider = services.BuildServiceProvider();
            var service = provider.GetService<IJwtService>();

            Assert.NotNull(service);
        }

        [Fact]
        public void WhenAddingInfrastructure_EmailServiceObjectMustNotBeNull()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x["Notification:EmailConfigurationString"]).Returns("MySystem.Api.UnitTest.Controllers.V1.CredentialsController");

            var services = new ServiceCollection();
            services.AddInfrastructure(configMock.Object);
            var provider = services.BuildServiceProvider();
            var service = provider.GetService<IEmailNotificationService>();

            Assert.NotNull(service);
        }
    }
}
