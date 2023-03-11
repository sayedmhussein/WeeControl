using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WeeControl.ApiApp.Persistence;
using WeeControl.Core.Domain.Interfaces;

namespace WeeControl.Host.Test.Api.DependencyInjections;

public class PersistenceTests
{
    private readonly IServiceCollection services;

    public PersistenceTests()
    {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(x => x.GetSection("ConnectionStrings")["DatabaseProvider"]).Returns("Connection");

        services = new ServiceCollection();
    }

    [Fact]
    public void WhenAddingPersistenceInMemory_ReturnEssentialDbContextObjectAsNotNull()
    {
        services.AddPersistenceAsInMemory();

        var service = services.BuildServiceProvider().GetService<IEssentialDbContext>();

        Assert.NotNull(service);
    }
}