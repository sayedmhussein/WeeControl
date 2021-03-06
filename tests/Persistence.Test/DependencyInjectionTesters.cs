using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Application.Interfaces;
using Xunit;

namespace WeeControl.Persistence.Test;

public class DependencyInjectionTesters : IDisposable
{
    private IServiceCollection services;
    public DependencyInjectionTesters()
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
    public void WhenAddingPresestenceInMemory_ReturnEssentialDbContextObjectAsNotNull()
    {
        services.AddPersistenceAsInMemory();
            
        var service = services.BuildServiceProvider().GetService<IEssentialDbContext>();

        Assert.NotNull(service);
    }
}