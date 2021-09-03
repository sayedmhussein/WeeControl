using System;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;
using WeeControl.SharedKernel.Aggregates.Employee.Enums;
using WeeControl.SharedKernel.Aggregates.Employee;
using WeeControl.Backend.WebApi.Services;
using MediatR;

namespace WeeControl.Backend.WebApi.Test.Services
{
    public class UserInfoServiceTests : IDisposable
    {
        private readonly IEmployeeLists employeeLists;
        private readonly Claim sessionClaim;
        private readonly Claim territoryClaim;
        
        private Mock<IHttpContextAccessor> httpContextMock;

        public UserInfoServiceTests()
        {
            employeeLists = new EmployeeLists();
            sessionClaim = new Claim(employeeLists.GetClaimType(ClaimTypeEnum.Session), Guid.NewGuid().ToString());
            territoryClaim = new Claim(employeeLists.GetClaimType(ClaimTypeEnum.Territory), Guid.NewGuid().ToString());

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
            var service = new UserInfoService(httpContextMock.Object, null, employeeLists);

            var claims = service.Claims;

            Assert.NotEmpty(claims);
        }

        [Fact]
        public void WhenSessionClaimInContext_SessionMustNotBeNull()
        {
            var service = new UserInfoService(httpContextMock.Object, null, employeeLists);

            var session = service.SessionId;

            Assert.NotNull(session);
            Assert.Equal(sessionClaim.Value, ((Guid)session).ToString());
        }

        [Fact]
        public void WhenSessionClaimInContextNotExist_SessionMustBeNull()
        {
            httpContextMock.Setup(x => x.HttpContext.User.Claims).Returns(new List<Claim>());
            var service = new UserInfoService(httpContextMock.Object, null, employeeLists);

            var session = service.SessionId;

            Assert.Null(session);
        }

        [Fact]
        public void WhenTerritoryClaimInContextExist_()
        {
            var mediatrMock = new Mock<IMediator>();

            var service = new UserInfoService(httpContextMock.Object, mediatrMock.Object, employeeLists);

            var territoties = service.Territories;

            Assert.NotEmpty(territoties);
        }
    }
}
