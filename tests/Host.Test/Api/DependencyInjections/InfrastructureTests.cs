using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.ApiApp.Infrastructure;
using WeeControl.Core.Application.Interfaces;

namespace WeeControl.Host.Test.Api.DependencyInjections;

public class InfrastructureTests
{
    [Fact]
    public void WhenAddingInfrastructure_EmailServiceObjectMustNotBeNull()
    {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(x => x.GetSection("ConnectionStrings")["EmailProvider"])
            .Returns("Connection String of Email Provider");

        var services = new ServiceCollection();
        services.AddInfrastructure(configMock.Object);
        var provider = services.BuildServiceProvider();

        var service = provider.GetService<IEmailNotificationService>();

        Assert.NotNull(service);
    }
}