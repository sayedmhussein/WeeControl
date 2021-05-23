using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MySystem.Web.EntityFramework;

namespace MySystem.Web.Api.UnitTest
{
    public static class TestObjects
    {
        #region DataContext
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
        #endregion

        #region Configuration
        public static IConfiguration Configuration => ConfigurationMock.Object;

        public static Mock<IConfiguration> ConfigurationMock
        {
            get
            {
                var configMock = new Mock<IConfiguration>();
                configMock.Setup(x => x["Jwt:Key"]).Returns("MySystem.Api.UnitTest.Controllers.V1.CredentialsController");
                return configMock;
            }
        }
        #endregion

        #region HttpContext
        public static HttpContext GetHttpContext(IEnumerable<Claim> claims)
        {
            var context = new Mock<HttpContext>();
            var identity = new ClaimsIdentity(claims);
            var principal = new ClaimsPrincipal(identity);
            context.Setup(c => c.User).Returns(principal);

            return context.Object;
        }
        #endregion

        #region HttpContextAccessor
        //public static IHttpContextAccessor HttpContextAccessor => HttpContextAccessorMock.Object;

        //public static Mock<IHttpContextAccessor> HttpContextAccessorMock
        //{
        //    get
        //    {
        //        var ca = new Mock<IHttpContextAccessor>();
        //        ca.Setup(x => x.HttpContext).Returns(c.Object);

        //        return ca;
        //    }
        //}
        #endregion HttpContextAccessor

    }
}
