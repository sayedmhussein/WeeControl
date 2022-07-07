using System.Threading;
using WeeControl.Application.Contexts.System.Queries;
using WeeControl.Application.Exceptions;
using WeeControl.SharedKernel.RequestsResponses;
using Xunit;

namespace WeeControl.Application.Test.System.Queries;

public class VerifyRequestHandlerTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData(0, 0)]
    [InlineData(-90, 0)]
    [InlineData(90, 0)]
    [InlineData(0, 180)]
    [InlineData(0, -180)]
    public async void WhenRequestIsValid_NoErrorBeThrown(double? latitude, double? longitude)
    {
        var query = new VerifyRequestQuery(RequestDto.Create("DeviceName", latitude, longitude));
        var handler = new VerifyRequestQuery.VerifyRequestHandler();

        var response = await handler.Handle(query, CancellationToken.None);

        Assert.IsType<MediatR.Unit>(response);
    }
    
    [Fact]
    public async void WhenRequestDeviceIsEmptyOrNull_ThrownException()
    {
        var query = new VerifyRequestQuery(RequestDto.Create(string.Empty, 0, 0));
        var handler = new VerifyRequestQuery.VerifyRequestHandler();

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(query, default)); ;
    }
    
    [Theory]
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
        var query = new VerifyRequestQuery(RequestDto.Create("device", latitude, longitude));
        var handler = new VerifyRequestQuery.VerifyRequestHandler();

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(query, default)); ;
    }
}