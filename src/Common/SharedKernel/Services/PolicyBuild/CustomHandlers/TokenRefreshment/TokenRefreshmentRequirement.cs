using Microsoft.AspNetCore.Authorization;

namespace WeeControl.SharedKernel.Services.PolicyBuild.CustomHandlers.TokenRefreshment;

public class TokenRefreshmentRequirement : IAuthorizationRequirement
{
    public TimeSpan Period { get; }

    public TokenRefreshmentRequirement(TimeSpan maximumPeriod)
    {
        Period = maximumPeriod;
    }
}