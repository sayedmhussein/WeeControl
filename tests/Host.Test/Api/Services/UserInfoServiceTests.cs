using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WeeControl.Core.SharedKernel;
using WeeControl.Host.WebApi.Services;

namespace WeeControl.Host.Test.Api.Services;

public class UserInfoServiceTests : IDisposable
{
    private readonly Claim sessionClaim;

    private Mock<IHttpContextAccessor> httpContextMock;

    public UserInfoServiceTests()
    {
        sessionClaim = new Claim(ClaimsValues.ClaimTypes.Session, Guid.NewGuid().ToString());
        var territoryClaim = new Claim(ClaimsValues.ClaimTypes.Territory, Guid.NewGuid().ToString());

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
        var service = new UserInfoServices.UserInfoService(httpContextMock.Object);

        var claims = service.Claims;

        Assert.NotEmpty(claims);
    }

    [Fact]
    public void WhenSessionClaimInTheContext_SessionMustNotBeNull()
    {
        var service = new UserInfoServices.UserInfoService(httpContextMock.Object);

        var session = service.SessionId;

        Assert.NotNull(session);
        Assert.Equal(sessionClaim.Value, ((Guid)session).ToString());
    }

    [Fact]
    public void WhenSessionClaimInTheContextNotExist_SessionMustBeNull()
    {
        httpContextMock.Setup(x => x.HttpContext.User.Claims).Returns(new List<Claim>());
        var service = new UserInfoServices.UserInfoService(httpContextMock.Object);

        var session = service.SessionId;

        Assert.Null(session);
    }
}