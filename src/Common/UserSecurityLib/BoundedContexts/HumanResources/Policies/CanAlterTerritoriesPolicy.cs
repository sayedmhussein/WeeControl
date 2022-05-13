using System;
using WeeControl.Common.UserSecurityLib.Helpers;
using WeeControl.Common.UserSecurityLib.Helpers.CustomHandlers.TokenRefreshment;

namespace WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources.Policies;

internal class CanAlterTerritoriesPolicy : PolicyBuilderBase
{
    internal CanAlterTerritoriesPolicy()
    {
        Builder.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
    }
}