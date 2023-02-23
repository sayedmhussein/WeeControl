using WeeControl.Core.Application.Behaviours;
using WeeControl.Core.Application.Exceptions;

namespace WeeControl.Core.Test.Application.Behaviours;

public class VerifyRequestHandlerTests
{
    [Theory(Skip = "Later")]
    [InlineData(null, null)]
    [InlineData(0, 0)]
    [InlineData(-90, 0)]
    [InlineData(90, 0)]
    [InlineData(0, 180)]
    [InlineData(0, -180)]
    public async void WhenRequestIsValid_NoErrorBeThrown(double? latitude, double? longitude)
    {
        var query = new TestExampleQuery(100);
        var handler = new RequestVerificationBehaviour<TestExampleQuery>();

        await handler.Process(query, CancellationToken.None);
    }
    
    [Fact(Skip = "Later")]
    public async void WhenRequestDeviceIsEmptyOrNull_ThrownException()
    {
        var query = new TestExampleQuery(100);
        var handler = new RequestVerificationBehaviour<TestExampleQuery>();

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Process(query, default)); ;
    }
    
    [Theory(Skip = "Later")]
    [InlineData(-90, null)]
    [InlineData(-90.1, 0)]
    [InlineData(90.1, null)]
    [InlineData(90.1, 0)]
    [InlineData(null, -180)]
    [InlineData(0, -180.1)]
    [InlineData(null, 180)]
    [InlineData(0, 180.1)]
    public async void WhenRequestGeoAreOutOfRange_ThrownException(double? latitude, double? longitude)
    {
        var query = new TestExampleQuery(100);
        var handler = new RequestVerificationBehaviour<TestExampleQuery>();

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Process(query, default)); ;
    }
}