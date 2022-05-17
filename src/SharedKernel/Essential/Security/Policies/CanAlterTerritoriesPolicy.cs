using WeeControl.SharedKernel.Essential.Security.CustomHandlers.TokenRefreshment;

namespace WeeControl.SharedKernel.Essential.Security.Policies;

internal class CanAlterTerritoriesPolicy : PolicyBuilderBase
{
    internal CanAlterTerritoriesPolicy()
    {
        Builder.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
    }
}