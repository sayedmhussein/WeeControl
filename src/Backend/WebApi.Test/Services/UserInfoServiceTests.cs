﻿using System;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;
using WeeControl.Backend.WebApi.Services;
using MediatR;
using WeeControl.Common.SharedKernel.EntityGroups.Employee.Attributes;
using WeeControl.Common.SharedKernel.EntityGroups.Employee.Enums;
using WeeControl.Common.SharedKernel.EntityGroups.Employee.Interfaces;
using WeeControl.Common.UserSecurityLib;
using WeeControl.Common.UserSecurityLib.Enums;
using WeeControl.Common.UserSecurityLib.Interfaces;

namespace WeeControl.Backend.WebApi.Test.Services
{
    public class UserInfoServiceTests : IDisposable
    {
        private readonly IClaimsTags employeeAttribute;
        private readonly Claim sessionClaim;
        private readonly Claim territoryClaim;
        
        private Mock<IHttpContextAccessor> httpContextMock;

        public UserInfoServiceTests()
        {
            employeeAttribute = new ClaimsTagsList();
            sessionClaim = new Claim(employeeAttribute.GetClaimType(ClaimTypeEnum.Session), Guid.NewGuid().ToString());
            territoryClaim = new Claim(employeeAttribute.GetClaimType(ClaimTypeEnum.Territory), Guid.NewGuid().ToString());

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
            var service = new UserInfoService(httpContextMock.Object, null, employeeAttribute);

            var claims = service.Claims;

            Assert.NotEmpty(claims);
        }

        [Fact]
        public void WhenSessionClaimInContext_SessionMustNotBeNull()
        {
            var service = new UserInfoService(httpContextMock.Object, null, employeeAttribute);

            var session = service.SessionId;

            Assert.NotNull(session);
            Assert.Equal(sessionClaim.Value, ((Guid)session).ToString());
        }

        [Fact]
        public void WhenSessionClaimInContextNotExist_SessionMustBeNull()
        {
            httpContextMock.Setup(x => x.HttpContext.User.Claims).Returns(new List<Claim>());
            var service = new UserInfoService(httpContextMock.Object, null, employeeAttribute);

            var session = service.SessionId;

            Assert.Null(session);
        }

        [Fact]
        public void WhenTerritoryClaimInContextExist_()
        {
            var mediatrMock = new Mock<IMediator>();

            var service = new UserInfoService(httpContextMock.Object, mediatrMock.Object, employeeAttribute);

            var territoties = service.Territories;

            Assert.NotEmpty(territoties);
        }
    }
}
