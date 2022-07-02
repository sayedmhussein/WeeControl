using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using WeeControl.Application.Behaviours;
using WeeControl.Application.Interfaces;
using Xunit;

namespace WeeControl.Application.Test.Behaviours;

public class RequestPerformanceBehaviourTests : IDisposable
{
    private Mock<ILogger<RequestPerformanceBehaviour<TestExampleQuery, string>>> loggerMock;
    private Mock<ICurrentUserInfo> userMock;
    private RequestHandlerDelegate<string> handlerDelegate;
    
    public RequestPerformanceBehaviourTests()
    {
        loggerMock = new Mock<ILogger<RequestPerformanceBehaviour<TestExampleQuery, string>>>();

        userMock = new Mock<ICurrentUserInfo>();
        userMock.Setup(x => x.GetSessionId()).Returns(Guid.NewGuid());
    }

    public void Dispose()
    {
        loggerMock = null;
        userMock = null;
        handlerDelegate = null;
    }
    
    [Fact(Skip = "Issue in this unit test!")]
    //[Fact]
    public async void HandlerShouldNotLogWarningIfTimeIsLess()
    {
        var behaviour = new RequestPerformanceBehaviour<TestExampleQuery, string>(loggerMock.Object, userMock.Object);
        handlerDelegate = async () =>
        {
            await Task.Delay(100);
            return "Completed";
        };

        var response = await behaviour.Handle(new TestExampleQuery(100), default, handlerDelegate);
        
        Assert.NotEmpty(response);
        loggerMock.Verify(x => 
            x.LogWarning(It.IsAny<string>()), Times.Never);
    }
    
    [Fact(Skip = "Issue in this unit test!")]
    public async void HandlerShouldLogWarningIfTimeIsHigh()
    {
        var behaviour = new RequestPerformanceBehaviour<TestExampleQuery, string>(loggerMock.Object, userMock.Object);
        handlerDelegate = async () =>
        {
            await Task.Delay(1000);
            return "Completed";
        };
        
        var response = await behaviour.Handle(new TestExampleQuery(1000), default, handlerDelegate);
        
        Assert.NotEmpty(response);
        loggerMock.Verify(x => 
            x.LogWarning(
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<long>(), 
                It.IsAny<Guid?>(), 
                It.IsAny<object>()), Times.AtLeastOnce);
    }
}