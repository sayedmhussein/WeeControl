using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MySystem.Application.Common.Interfaces;
using MySystem.SharedKernel.Definition;

namespace MySystem.Api.Service
{
    public class CurrentUserInfoService : ICurrentUserInfo
    {
        public CurrentUserInfoService(IHttpContextAccessor httpContextAccessor)
        {
            Claims = httpContextAccessor.HttpContext.User.Claims;
            var sessionid = Claims.FirstOrDefault(c => c.Type == UserClaim.Session)?.Value;

            _ = Guid.TryParse(sessionid, out Guid SessionId);
        }

        public Guid SessionId { get; private set; }

        public IEnumerable<Claim> Claims { get; private set; }
    }
}
