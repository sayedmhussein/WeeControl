using Microsoft.AspNetCore.Authorization;

namespace WeeControl.SharedKernel.Essential.Security.CustomHandlers.TokenRefreshment;

public class TokenRefreshmentRequirement : IAuthorizationRequirement
{
    public TimeSpan Period { get; }

    public TokenRefreshmentRequirement(TimeSpan maximumPeriod)
    {
        Period = maximumPeriod;
    }
}