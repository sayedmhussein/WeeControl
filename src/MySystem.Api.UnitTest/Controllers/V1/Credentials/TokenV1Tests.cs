using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Sayed.MySystem.Api.Controllers.V1;
using Sayed.MySystem.EntityFramework;
using Xunit;

namespace Sayed.MySystem.Api.UnitTest.Controllers.V1.Credentials
{
    public class TokenV1Tests
    {
        private readonly ILogger<CredentialsController> logger = new Mock<ILogger<CredentialsController>>().Object;

    }
}
