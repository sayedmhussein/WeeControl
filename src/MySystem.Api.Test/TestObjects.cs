using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Sayed.MySystem.EntityFramework;

namespace Sayed.MySystem.Api.UnitTest
{
    public static class TestObjects
    {
        public static Mock<IConfiguration> ConfigurationMock
        {
            get
            {
                var configMock = new Mock<IConfiguration>();
                configMock.Setup(x => x["Jwt:Key"]).Returns("MySystem.Api.UnitTest.Controllers.V1.CredentialsController");
                return configMock;
            }
        }

        public static IConfiguration Configuration => ConfigurationMock.Object;

        public static Mock<IHttpContextAccessor> HttpContextAccessorMock
        {
            get
            {
                var c = new Mock<HttpContext>();
                var ca = new Mock<IHttpContextAccessor>();
                ca.Setup(x => x.HttpContext).Returns(c.Object);

                return ca;
            }
        }

        public static IHttpContextAccessor HttpContextAccessor => HttpContextAccessorMock.Object;
        
        public static DataContext DataContext
        {
            get
            {
                var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: new Random().NextDouble().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
                return new DataContext(options);
            }
        }
    }
}
