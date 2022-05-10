using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Domain.Interfaces;
using WeeControl.Backend.Infrastructure;
using Xunit;

namespace WeeControl.test.Infrastructure.Test;

public class DependencyInjectionTesters
{
    [Fact]
    public void WhenAddingInfrastructure_EmailServiceObjectMustNotBeNull()
    {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(x => x.GetSection("ConnectionStrings")["EmailProvider"])
            .Returns("MySystem.Api.UnitTest.Controllers.V1.CredentialsController");

        var services = new ServiceCollection();
        services.AddInfrastructure(configMock.Object);
        var provider = services.BuildServiceProvider();
        var service = provider.GetService<IEmailNotificationService>();

        Assert.NotNull(service);
    }
}