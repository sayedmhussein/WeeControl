using Microsoft.AspNetCore.Authorization;
using WeeControl.Common.UserSecurityLib.BoundedContexts;

namespace WeeControl.Common.UserSecurityLib.Helpers
{
    public abstract class PolicyBuilderBase
    {
        protected readonly AuthorizationPolicyBuilder Builder;

        protected PolicyBuilderBase()
        {
            Builder = new AuthorizationPolicyBuilder();
            Builder.AddAuthenticationSchemes("Bearer");
            Builder.RequireClaim(SecurityClaims.HumanResources.Session);
        }

        public Microsoft.AspNetCore.Authorization.AuthorizationPolicy GetPolicy()
        {
            return Builder.Build();
        }
    }
}