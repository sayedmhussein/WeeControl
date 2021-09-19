using System;
using WeeControl.UserSecurityLib.Helpers;
using WeeControl.UserSecurityLib.Helpers.CustomHandlers.TokenRefreshment;

namespace WeeControl.UserSecurityLib.Policies.Territory
{
    public class CanAlterTerritoriesPolicy : PolicyBuilderBase
    {
        public CanAlterTerritoriesPolicy()
        {
            Builder.Requirements.Add(new TokenRefreshmentRequirement(TimeSpan.FromMinutes(5)));
        }
    }
}