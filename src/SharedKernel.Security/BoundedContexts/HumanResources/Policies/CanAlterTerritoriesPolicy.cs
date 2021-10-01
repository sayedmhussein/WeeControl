using System;
using WeeControl.SharedKernel.Security.Helpers;
using WeeControl.SharedKernel.Security.Helpers.CustomHandlers.TokenRefreshment;

namespace WeeControl.SharedKernel.Security.BoundedContexts.HumanResources.Policies
{
    internal class CanAlterTerritoriesPolicy : PolicyBuilderBase
    {
        internal CanAlterTerritoriesPolicy()
        {
            Builder.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
        }
    }
}