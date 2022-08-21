using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.ApiApp.Domain.Interfaces;
using Xunit;

namespace WeeControl.ApiApp.Infrastructure.Test;

public class DependencyInjectionTesters
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