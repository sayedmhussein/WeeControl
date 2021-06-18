using System;
using Microsoft.AspNetCore.Authorization;

namespace WeeControl.Server.WebApi.Security.TokenRefreshment.CustomHandlers
{
    public class TokenRefreshmentRequirement : IAuthorizationRequirement
    {
        public TimeSpan Period { get; }

        public TokenRefreshmentRequirement(TimeSpan maximumPeriod)
        {
            Period = maximumPeriod;
        }
    }
}
