using Microsoft.AspNetCore.Authorization;
using WeeControl.SharedKernel.Security.BoundedContexts.HumanResources;

namespace WeeControl.SharedKernel.Security.Helpers
{
    public abstract class PolicyBuilderBase
    {
        protected readonly AuthorizationPolicyBuilder Builder;

        protected PolicyBuilderBase()
        {
            Builder = new AuthorizationPolicyBuilder();
            Builder.AddAuthenticationSchemes("Bearer");
            Builder.RequireClaim(HumanResourcesData.Claims.Session);
        }

        public Microsoft.AspNetCore.Authorization.AuthorizationPolicy GetPolicy()
        {
            return Builder.Build();
        }
    }
}