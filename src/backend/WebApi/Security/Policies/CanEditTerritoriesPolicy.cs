using System;
using WeeControl.WebApi.Security.CustomHandlers.TokenRefreshment;

namespace WeeControl.WebApi.Security.Policies;

internal class CanEditTerritoriesPolicy : PolicyBuilderBase
{
    public const string Name = nameof(CanEditTerritoriesPolicy);
    
    internal CanEditTerritoriesPolicy()
    {
        Builder.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
    }
}