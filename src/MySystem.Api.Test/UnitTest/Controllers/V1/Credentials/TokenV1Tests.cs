using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Sayed.MySystem.Api.Controllers.V1;
using Sayed.MySystem.EntityFramework;
using Xunit;

namespace Sayed.MySystem.Api.UnitTest.Controllers.V1.Credentials
{
    public class TokenV1Tests : IDisposable
    {
        private ILogger<CredentialsController> logger;
        private CredentialsController controller;

        public TokenV1Tests()
        {
            logger = new Mock<ILogger<CredentialsController>>().Object;
            controller = new CredentialsController(logger, TestObjects.DataContext, TestObjects.Configuration, TestObjects.HttpContextAccessor);
        }

        public void Dispose()
        {
            controller = null;
            logger = null;
        }

        [Fact]
        public void WhenPassingValidTokenWhichHasValidSessionClaim_ReturnNewToken()
        {

        }
    }
}
