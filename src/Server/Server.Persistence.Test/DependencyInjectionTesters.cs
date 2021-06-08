using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using MySystem.Persistence;
using MySystem.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;

namespace MySystem.Persistence.Test
{
    public class DependencyInjectionTesters
    {
        [Fact]
        public void WhenAddingPresistanceInMemory_ReturnMySystemDbContextObjectAsNotNull()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x["DbConnection"]).Returns("Connection");

            var services = new ServiceCollection();
            services.AddPersistenceAsInMemory("Name");
            var provider = services.BuildServiceProvider();
            var service = provider.GetService<IMySystemDbContext>();

            Assert.NotNull(service);
        }
    }
}
