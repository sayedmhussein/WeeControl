using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using WeeControl.UserSecurityLib.Policies;

namespace WeeControl.UserSecurityLib.Helpers
{
    public abstract class PolicyBuilderBase
    {
        protected readonly AuthorizationPolicyBuilder Builder;

        protected PolicyBuilderBase()
        {
            Builder = new AuthorizationPolicyBuilder();
            Builder.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            Builder.RequireClaim(ClaimGroup.ClaimType.Session);
        }

        public AuthorizationPolicy GetPolicy()
        {
            return Builder.Build();
        }
    }
}