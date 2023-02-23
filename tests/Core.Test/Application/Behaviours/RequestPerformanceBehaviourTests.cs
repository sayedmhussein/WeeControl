using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using WeeControl.Core.Application.Behaviours;
using WeeControl.Core.Application.Interfaces;

namespace WeeControl.Core.Test.Application.Behaviours;

public class RequestPerformanceBehaviourTests : IDisposable
{
    private readonly Mock<ILogger<RequestPerformanceBehaviour<TestExampleQuery, Unit>>> loggerMock;
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
    [InlineData(10000, true)]
    public async void HandlerShouldNotLogWarningIfTimeIsLess(int ms, bool isDelay)
    {
        var m = new Mock<ILogger>();
        
        var behaviour = new RequestPerformanceBehaviour<TestExampleQuery, Unit>(loggerMock.Object, userMock?.Object);
        
        await behaviour.Handle(new TestExampleQuery(ms), () => Unit.Task, default);

        Assert.NotNull(loggerMock.Object);
        
        loggerMock.Verify(x => x.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), isDelay ? Times.Once : Times.Never);
    }
}