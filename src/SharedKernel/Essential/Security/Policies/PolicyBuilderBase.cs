using Microsoft.AspNetCore.Authorization;

namespace WeeControl.SharedKernel.Essential.Security.Policies;

public abstract class PolicyBuilderBase
{
    protected readonly AuthorizationPolicyBuilder Builder;

    protected PolicyBuilderBase()
    {
        Builder = new AuthorizationPolicyBuilder();
        Builder.AddAuthenticationSchemes("Bearer");
        Builder.RequireClaim(ClaimsTagsList.Claims.Session);
    }

    internal AuthorizationPolicy GetPolicy()
    {
        return Builder.Build();
    }
}