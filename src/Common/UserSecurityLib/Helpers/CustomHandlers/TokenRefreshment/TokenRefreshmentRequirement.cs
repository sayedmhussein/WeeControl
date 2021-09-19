using System;
using Microsoft.AspNetCore.Authorization;

namespace WeeControl.UserSecurityLib.Helpers.CustomHandlers.TokenRefreshment
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
