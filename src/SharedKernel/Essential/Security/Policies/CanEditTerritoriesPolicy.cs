using WeeControl.SharedKernel.Essential.Security.CustomHandlers.TokenRefreshment;

namespace WeeControl.SharedKernel.Essential.Security.Policies;

internal class CanEditTerritoriesPolicy : PolicyBuilderBase
{
    public const string Name = nameof(CanEditTerritoriesPolicy);
    
    internal CanEditTerritoriesPolicy()
    {
        Builder.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
    }
}