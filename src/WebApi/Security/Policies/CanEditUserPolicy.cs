using WeeControl.SharedKernel;

namespace WeeControl.WebApi.Security.Policies;

internal class CanEditUserPolicy : PolicyBuilderBase
{
    public CanEditUserPolicy()
    {
        Builder.RequireClaim(ClaimsValues.ClaimTypes.Administrator);
    }
}