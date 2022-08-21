using WeeControl.SharedKernel;

namespace WeeControl.ApiApp.WebApi.Security.Policies;

internal class CanEditUserPolicy : PolicyBuilderBase
{
    public CanEditUserPolicy()
    {
        Builder.RequireClaim(ClaimsValues.ClaimTypes.Administrator);
    }
}