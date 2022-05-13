using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using WeeControl.Backend.WebApi.Services;
using WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources;
using Xunit;

namespace WeeControl.test.WebApi.Test.Services;

public class UserInfoServiceTests : IDisposable
{
    private readonly Claim sessionClaim;
    private readonly Claim territoryClaim;
        
    private Mock<IHttpContextAccessor> httpContextMock;

    public UserInfoServiceTests()
    {
        sessionClaim = new Claim(HumanResourcesData.Claims.Session, Guid.NewGuid().ToString());
        territoryClaim = new Claim(HumanResourcesData.Claims.Territory, Guid.NewGuid().ToString());

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
        mediatrMock.Setup(x => 
                x.Send(It.IsAny<object>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<string>() { "string1", "string2" });

        var service = new UserInfoService(httpContextMock.Object, mediatrMock.Object);

        var territoties = await service.GetTerritoriesListAsync();

        Assert.NotEmpty(territoties);
    }
}