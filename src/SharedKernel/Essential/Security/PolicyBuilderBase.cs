using Microsoft.AspNetCore.Authorization;
using WeeControl.SharedKernel.Essential;
using WeeControl.SharedKernel.Essential.Security;

namespace WeeControl.SharedKernel.Services.PolicyBuild;

public abstract class PolicyBuilderBase
{
    protected readonly AuthorizationPolicyBuilder Builder;

    protected PolicyBuilderBase()
    {
        Builder = new AuthorizationPolicyBuilder();
        Builder.AddAuthenticationSchemes("Bearer");
        Builder.RequireClaim(ClaimsTagsList.Claims.Session);
    }

    public Microsoft.AspNetCore.Authorization.AuthorizationPolicy GetPolicy()
    {
        return Builder.Build();
    }
}