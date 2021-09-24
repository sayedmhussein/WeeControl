using System;
using WeeControl.Common.UserSecurityLib.Helpers;
using WeeControl.Common.UserSecurityLib.Helpers.CustomHandlers.TokenRefreshment;

namespace WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources.Policies
{
    public class CanAlterTerritoriesPolicy : PolicyBuilderBase
    {
        public CanAlterTerritoriesPolicy()
        {
            Builder.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
        }
    }
}