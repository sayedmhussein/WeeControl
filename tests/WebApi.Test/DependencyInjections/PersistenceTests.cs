using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.ApiApp.Application.Interfaces;
using WeeControl.ApiApp.Persistence;
using Xunit;

namespace WeeControl.ApiApp.WebApi.Test.DependencyInjections;

public class PersistenceTests
{
    private IServiceCollection services;
    public PersistenceTests()
    {
        var configMock = new Mock<IConfiguration>();
        configMock.Setup(x => x.GetSection("ConnectionStrings")["DatabaseProvider"]).Returns("Connection");

        services = new ServiceCollection();
    }
        
    public void Dispose()
    {
        services = null;
    }
    
    [Fact]
    public void WhenAddingPersistenceInMemory_ReturnEssentialDbContextObjectAsNotNull()
    {
        services.AddPersistenceAsInMemory();
            
        var service = services.BuildServiceProvider().GetService<IEssentialDbContext>();

        Assert.NotNull(service);
    }
}