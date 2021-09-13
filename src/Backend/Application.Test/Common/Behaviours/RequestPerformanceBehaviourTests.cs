using Microsoft.Extensions.Logging;
using Moq;
using WeeControl.Backend.Application.Common.Behaviours;
using WeeControl.Backend.Application.Common.Interfaces;
using Xunit;

namespace WeeControl.Backend.Application.Test.Common.Behaviours
{
    public class RequestPerformanceBehaviourTests
    {
        [Fact]
        public async void HandleShouldReturnTimeBetweenRequestAndResponse()
        {
            var loggerMock = new Mock<ILogger<string>>();
            var userMock = new Mock<ICurrentUserInfo>();

            var behaviour = new RequestPerformanceBehaviour<string, string>(loggerMock.Object, userMock.Object);
            //var requestHandler = new RequestHandlerDelegate()

            //var response = await behaviour.Handle("request", default, "");
        }
    }
}
