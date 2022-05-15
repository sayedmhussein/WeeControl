using WeeControl.Common.SharedKernel.Services.PolicyBuild;
using WeeControl.Common.SharedKernel.Services.PolicyBuild.CustomHandlers.TokenRefreshment;

namespace WeeControl.Common.SharedKernel.Essential.Policies;

internal class CanAlterTerritoriesPolicy : PolicyBuilderBase
{
    internal CanAlterTerritoriesPolicy()
    {
        Builder.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
    }
}