using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Backend.Domain.Interfaces;
using Xunit;

namespace WeeControl.Backend.Persistence.Test
{
    public class DependencyInjectionTesters
    {
        [Fact]
        public void WhenAddingPresistanceInMemory_ReturnMySystemDbContextObjectAsNotNull()
        {
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x.GetSection("ConnectionStrings")["DatabaseProvider"]).Returns("Connection");

            var services = new ServiceCollection();
            services.AddPersistenceAsInMemory("Name");
            var provider = services.BuildServiceProvider();
            var service = provider.GetService<IMySystemDbContext>();

            Assert.NotNull(service);
        }
    }
}
