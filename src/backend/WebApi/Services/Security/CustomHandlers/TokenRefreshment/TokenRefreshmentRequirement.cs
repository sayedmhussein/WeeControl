using Microsoft.AspNetCore.Authorization;
using System;

namespace WeeControl.ApiApp.WebApi.Services.Security.CustomHandlers.TokenRefreshment;

public class TokenRefreshmentRequirement : IAuthorizationRequirement
{
    public TimeSpan Period { get; }

    public TokenRefreshmentRequirement(TimeSpan maximumPeriod)
    {
        Period = maximumPeriod;
    }
}