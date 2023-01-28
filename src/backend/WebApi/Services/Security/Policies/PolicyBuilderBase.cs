using Microsoft.AspNetCore.Authorization;
using WeeControl.Core.SharedKernel;

namespace WeeControl.ApiApp.WebApi.Services.Security.Policies;

public abstract class PolicyBuilderBase
{
    protected readonly AuthorizationPolicyBuilder Builder;

    protected PolicyBuilderBase()
    {
        Builder = new AuthorizationPolicyBuilder();
        Builder.AddAuthenticationSchemes("Bearer");
        Builder.RequireClaim(ClaimsValues.ClaimTypes.Session);
    }

    internal AuthorizationPolicy GetPolicy()
    {
        return Builder.Build();
    }
}