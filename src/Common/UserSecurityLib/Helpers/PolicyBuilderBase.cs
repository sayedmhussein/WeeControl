using Microsoft.AspNetCore.Authorization;

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