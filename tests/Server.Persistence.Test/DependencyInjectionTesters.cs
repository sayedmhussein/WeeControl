using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using WeeControl.Server.Domain.Administration;
using WeeControl.Server.Domain.Authorization;
using WeeControl.Server.Domain.HumanResources;
using Xunit;

namespace WeeControl.Server.Persistence.Test
{
    public class DependencyInjectionTesters : IDisposable
    {
        private IServiceCollection services;

        public DependencyInjectionTesters()
        {
            services = new ServiceCollection();
        }
        
        public void Dispose()
        {
            services = null;
        }
        
        [Fact]
        public void WhenAddPersistenceAsPostgreSql_AllDbContextsAreNotNull()
        {
            var cs = "Server=127.0.0.1;Port=5432;Database=weecontrol_humanresources;Username=sayed;password=sayed;Include Error Detail = true";
            var configMock = new Mock<IConfiguration>();
            configMock.Setup(x => x.GetSection("ConnectionStrings")[It.IsAny<string>()]).Returns(cs);
            
            services.AddPersistenceAsPostgreSql(configMock.Object, nameof(DependencyInjectionTesters));

            Assert.NotNull(services.BuildServiceProvider().GetService<IAdministrationDbContext>());
            Assert.NotNull(services.BuildServiceProvider().GetService<IAuthorizationDbContext>());
            Assert.NotNull(services.BuildServiceProvider().GetService<IHumanResourcesDbContext>());
        }

        
    }
}
