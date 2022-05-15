using WeeControl.SharedKernel.Services.PolicyBuild;
using WeeControl.SharedKernel.Services.PolicyBuild.CustomHandlers.TokenRefreshment;

namespace WeeControl.SharedKernel.Essential.Policies;

internal class CanAlterTerritoriesPolicy : PolicyBuilderBase
{
    internal CanAlterTerritoriesPolicy()
    {
        Builder.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
    }
}