//using System;
//using System.Collections.Generic;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using Moq;
//using MySystem.Persistence.Api.UnitTest;
//using MySystem.Web.Api.Controllers.V1;
//using Xunit;

//namespace MySystem.Persistence.Api.Test.UnitTest.Controllers.V1.Credentials
//{
//    public class TokenV1Tests : IDisposable
//    {
//        private ILogger<CredentialsController> logger;
//        private CredentialsController controller;

//        public TokenV1Tests()
//        {
//            logger = new Mock<ILogger<CredentialsController>>().Object;

//            var claims = new List<Claim>();
//            var accessor = new Mock<IHttpContextAccessor>();
//            accessor.Setup(x => x.HttpContext).Returns(TestObjects.GetHttpContext(claims));
            
//            controller = new CredentialsController(TestObjects.DataContext, TestObjects.Configuration, null, logger);
//        }

//        public void Dispose()
//        {
//            controller = null;
//            logger = null;
//        }

//        [Fact]
//        public void WhenPassingValidTokenWhichHasValidSessionClaim_ReturnNewToken()
//        {

//        }
//    }
//}
