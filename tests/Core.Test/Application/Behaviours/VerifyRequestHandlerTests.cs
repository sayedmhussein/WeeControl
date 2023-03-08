using MediatR;
using WeeControl.Core.Application.Behaviours;
using WeeControl.Core.Application.Exceptions;
using WeeControl.Core.DataTransferObject.BodyObjects;
using WeeControl.Core.DataTransferObject.Contexts.Essentials;
using WeeControl.Core.SharedKernel;
using WeeControl.Core.SharedKernel.Exceptions;

namespace WeeControl.Core.Test.Application.Behaviours;

public class VerifyRequestHandlerTests
{
    [Theory]
    [InlineData(null, null)]
    [InlineData(0, 0)]
    [InlineData(-90, 0)]
    [InlineData(90, 0)]
    [InlineData(0, 180)]
    [InlineData(0, -180)]
    public async void WhenRequestIsValid_NoErrorBeThrown(double latitude, double longitude)
    {
        var dto = RequestDto.Create("device", latitude, longitude);
        var query = new TestRequestAsRequestDto(dto);
        var handler = new RequestVerificationBehaviour<TestRequestAsRequestDto>();

        await handler.Process(query, CancellationToken.None);
    }
    
    [Fact]
    //[Fact(Skip = "Later")]
    public async void WhenRequestDeviceIsEmptyOrNull_ThrownException()
    {
        var dto = RequestDto.Create("", null, null);
        var query = new TestRequestAsRequestDto(dto);
        var handler = new RequestVerificationBehaviour<TestRequestAsRequestDto>();

        await Assert.ThrowsAsync<EntityModelValidationException>(() => handler.Process(query, default)); ;
    }
    
    [Fact]
    public async void WhenNotRequestDto_PassNormal()
    {
        var dto = LoginRequestDto.Create("username", "password");
        var query = new TestRequestAsNotRequestDto(dto);
        var handler = new RequestVerificationBehaviour<TestRequestAsNotRequestDto>();

        await handler.Process(query, CancellationToken.None);
    }
    
    [Theory]
    [InlineData(-90.1, 0)]
    [InlineData(90.1, 0)]
    [InlineData(0, -180.1)]
    [InlineData(0, 180.1)]
    public async void WhenRequestGeoAreOutOfRange_ThrownException(double latitude, double longitude)
    {
        var dto = RequestDto.Create("device", latitude, longitude);
        var query = new TestRequestAsRequestDto(dto);
        var handler = new RequestVerificationBehaviour<TestRequestAsRequestDto>();

        await Assert.ThrowsAsync<EntityModelValidationException>(() => handler.Process(query, default)); ;
    }
}

internal class TestRequestAsRequestDto : RequestDto, IRequest
{
    public TestRequestAsRequestDto(RequestDto dto) : base(dto)
    {
    }
}

internal class TestRequestAsNotRequestDto : IRequest
{
    private LoginRequestDto login;
    public TestRequestAsNotRequestDto(LoginRequestDto dto)
    {
        login = dto;
    }
}