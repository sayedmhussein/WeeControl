using System;
using Microsoft.AspNetCore.Authorization;

namespace WeeControl.Host.WebApi.Services.Security.CustomHandlers.TokenRefreshment;

public class TokenRefreshmentRequirement : IAuthorizationRequirement
{
    public TokenRefreshmentRequirement(TimeSpan maximumPeriod)
    {
        Period = maximumPeriod;
    }

    public TimeSpan Period { get; }
}