using System;
using WeeControl.Host.WebApi.Services.Security.CustomHandlers.TokenRefreshment;

namespace WeeControl.Host.WebApi.Services.Security.Policies;

internal class CanEditTerritoriesPolicy : PolicyBuilderBase
{
    public const string Name = nameof(CanEditTerritoriesPolicy);

    internal CanEditTerritoriesPolicy()
    {
        Builder.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
    }
}