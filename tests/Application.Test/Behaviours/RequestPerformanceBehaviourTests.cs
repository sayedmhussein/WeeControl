using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using WeeControl.ApiApp.Application.Behaviours;
using WeeControl.ApiApp.Application.Interfaces;
using Xunit;

namespace WeeControl.ApiApp.Application.Test.Behaviours;

public class RequestPerformanceBehaviourTests : IDisposable
{
    private Mock<ILogger<RequestPerformanceBehaviour<TestExampleQuery, bool>>> loggerMock;
    private Mock<ICurrentUserInfo> userMock;
    private RequestHandlerDelegate<bool> handlerDelegate;
    
    public RequestPerformanceBehaviourTests()
    {
        loggerMock = new Mock<ILogger<RequestPerformanceBehaviour<TestExampleQuery, bool>>>();

        userMock = new Mock<ICurrentUserInfo>();
        userMock.Setup(x => x.SessionId).Returns(Guid.NewGuid());
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
        var behaviour = new RequestPerformanceBehaviour<TestExampleQuery, bool>(loggerMock.Object, userMock.Object);
        handlerDelegate = async () =>
        {
            await Task.Delay(100);
            return true;
        };

        var response = await behaviour.Handle(new TestExampleQuery(true), default, handlerDelegate);
        
        
    }
    
    [Fact(Skip = "Issue in this unit test!")]
    public async void HandlerShouldLogWarningIfTimeIsHigh()
    {
        var behaviour = new RequestPerformanceBehaviour<TestExampleQuery, bool>(loggerMock.Object, userMock.Object);
        handlerDelegate = async () =>
        {
            await Task.Delay(1000);
            return false;
        };
        
        var response = await behaviour.Handle(new TestExampleQuery(false), default, handlerDelegate);
        
    }
}