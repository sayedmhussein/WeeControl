using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using WeeControl.Application.EssentialContext.Queries;
using WeeControl.SharedKernel;
using WeeControl.SharedKernel.DataTransferObjects;
using WeeControl.SharedKernel.RequestsResponses;
using WeeControl.WebApi.Services;
using Xunit;

namespace WeeControl.WebApi.Test.Services;

public class UserInfoServiceTests : IDisposable
{
    private readonly Claim sessionClaim;
    private readonly Claim territoryClaim;
        
    private Mock<IHttpContextAccessor> httpContextMock;

    public UserInfoServiceTests()
    {
        sessionClaim = new Claim(ClaimsTagsList.Claims.Session, Guid.NewGuid().ToString());
        territoryClaim = new Claim(ClaimsTagsList.Claims.Territory, Guid.NewGuid().ToString());

        var claims = new List<Claim>()
        {
            sessionClaim,
            territoryClaim
        };

        httpContextMock = new Mock<IHttpContextAccessor>();
        httpContextMock.Setup(x => x.HttpContext.User.Claims).Returns(claims);
    }

    public void Dispose()
    {
        httpContextMock = null;
    }

    [Fact]
    public void WhenThereAreClaimsInContext_CountMustNotBeZero()
    {
        var service = new UserInfoService(httpContextMock.Object, null);

        var claims = service.Claims;

        Assert.NotEmpty(claims);
    }

    [Fact]
    public void WhenSessionClaimInContext_SessionMustNotBeNull()
    {
        var service = new UserInfoService(httpContextMock.Object, null);

        var session = service.GetSessionId();

        Assert.NotNull(session);
        Assert.Equal(sessionClaim.Value, ((Guid)session).ToString());
    }

    [Fact]
    public void WhenSessionClaimInContextNotExist_SessionMustBeNull()
    {
        httpContextMock.Setup(x => x.HttpContext.User.Claims).Returns(new List<Claim>());
        var service = new UserInfoService(httpContextMock.Object, null);

        var session = service.GetSessionId();

        Assert.Null(session);
    }

    [Fact]
    public async void WhenTerritoryClaimInContextExist_()
    {
        var mediatrMock = new Mock<IMediator>();
        mediatrMock.Setup(x => x
                .Send(It.IsAny<GetListOfTerritoriesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResponseDto<IEnumerable<TerritoryDto>>(new List<TerritoryDto>()
            {
                new() { TerritoryCode = "cod1"}
            }));

        var service = new UserInfoService(httpContextMock.Object, mediatrMock.Object);

        var territoties = await service.GetTerritoriesListAsync(default);

        Assert.NotEmpty(territoties);
    }
}