using System;
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
        public static IConfiguration Configuration
        {
            get
            {
                var configMock = new Mock<IConfiguration>();
                configMock.Setup(x => x["Jwt:Key"]).Returns("MySystem.Api.UnitTest.Controllers.V1.CredentialsController");
                return configMock.Object;
            }
        }

        public static DataContext DataContext
        {
            get
            {
                var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "mydbname")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
                return new DataContext(options);
            }
        }
    }
}
