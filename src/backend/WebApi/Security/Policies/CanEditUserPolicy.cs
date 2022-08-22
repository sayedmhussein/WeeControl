using WeeControl.Common.SharedKernel;

namespace WeeControl.ApiApp.WebApi.Security.Policies;

internal class CanEditUserPolicy : PolicyBuilderBase
{
    public CanEditUserPolicy()
    {
        Builder.RequireClaim(ClaimsValues.ClaimTypes.Administrator);
    }
}