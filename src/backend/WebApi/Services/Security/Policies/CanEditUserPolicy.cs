using WeeControl.Core.SharedKernel;

namespace WeeControl.Host.WebApi.Services.Security.Policies;

internal class CanEditUserPolicy : PolicyBuilderBase
{
    public CanEditUserPolicy()
    {
        Builder.RequireClaim(ClaimsValues.ClaimTypes.Administrator);
    }
}