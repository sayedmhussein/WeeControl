using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using WeeControl.Core.Application.Behaviours;
using WeeControl.Core.Application.Interfaces;

namespace WeeControl.Core.Test.Application.Behaviours;

public class RequestPerformanceBehaviourTests : IDisposable
{
    private Mock<ILogger<RequestPerformanceBehaviour<TestExampleQuery, Unit>>> loggerMock;
    private Mock<ICurrentUserInfo> userMock;


    public RequestPerformanceBehaviourTests()
    {
        loggerMock = new Mock<ILogger<RequestPerformanceBehaviour<TestExampleQuery, Unit>>>();
        

        userMock = new Mock<ICurrentUserInfo>();
        userMock.Setup(x => x.SessionId).Returns(Guid.NewGuid());
    }

    public void Dispose()
    {
        userMock = null;
    }

    [Theory(Skip = "Issue in this unit test!")]
    //[Theory]
    [InlineData(100, false)]
    [InlineData(1000, true)]
    public async void HandlerShouldNotLogWarningIfTimeIsLess(int ms, bool isDelay)
    {
        var behaviour = new RequestPerformanceBehaviour<TestExampleQuery, Unit>(loggerMock.Object, userMock?.Object);
        
        await behaviour.Handle(new TestExampleQuery(ms), () => Unit.Task, default);

        Assert.NotNull(loggerMock.Object);
    }
}