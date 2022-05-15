using System.Threading;
using WeeControl.Application.CommonContext.Queries;
using WeeControl.Application.Exceptions;
using WeeControl.SharedKernel.RequestsResponses;
using Xunit;

namespace WeeControl.test.Application.Test.CommonContext.Queries;

public class VerifyRequestHandlerTests
{
    [Fact]
    public async void WhenRequestIsValid_NoErrorBeThrown()
    {
        var query = new VerifyRequestQuery(new RequestDto("DeviceName"));
        var handler = new VerifyRequestHandler();

        var response = await handler.Handle(query, CancellationToken.None);

        Assert.IsType<MediatR.Unit>(response);
    }
    
    [Fact]
    public async void WhenRequestDeviceIsEmptyOrNull_ThrownException()
    {
        var query = new VerifyRequestQuery(new RequestDto(string.Empty));
        var handler = new VerifyRequestHandler();

        await Assert.ThrowsAsync<BadRequestException>(() => handler.Handle(query, CancellationToken.None)); ;
    }
}