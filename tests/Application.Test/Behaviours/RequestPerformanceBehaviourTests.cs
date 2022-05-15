using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using WeeControl.Application.Interfaces;
using Xunit;

namespace WeeControl.Application.Test.Behaviours;

public class RequestPerformanceBehaviourTests
{
    [Fact(Skip = "Issue in this unit test!")]
    public void HandleShouldReturnTimeBetweenRequestAndResponse()
    {
        var loggerMock = new Mock<IRequest<ILogger<string>>>();
        var userMock = new Mock<ICurrentUserInfo>();

        //var behaviour = new RequestPerformanceBehaviour<IRequest<ILogger<string>>, ICurrentUserInfo>(loggerMock.Object, userMock.Object);
        //var behaviour = new RequestPerformanceBehaviour<string, string>(loggerMock.Object, userMock.Object);
        //var requestHandler = new RequestHandlerDelegate()

        //var response = await behaviour.Handle("request", default, "");
    }
}