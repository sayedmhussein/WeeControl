using System;
using WeeControl.ApiApp.WebApi.Security.CustomHandlers.TokenRefreshment;

namespace WeeControl.ApiApp.WebApi.Security.Policies;

internal class CanEditTerritoriesPolicy : PolicyBuilderBase
{
    public const string Name = nameof(CanEditTerritoriesPolicy);
    
    internal CanEditTerritoriesPolicy()
    {
        Builder.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
    }
}