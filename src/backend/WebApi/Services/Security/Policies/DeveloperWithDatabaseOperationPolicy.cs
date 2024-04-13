using WeeControl.Core.SharedKernel;

namespace WeeControl.Host.WebApi.Services.Security.Policies;

public class DeveloperWithDatabaseOperationPolicy : PolicyBuilderBase
{
    public const string Name = nameof(DeveloperWithDatabaseOperationPolicy);

    public DeveloperWithDatabaseOperationPolicy()
    {
        Builder.RequireClaim(ClaimsValues.ClaimTypes.Developer);
        //Builder.Requirements.Add();
    }
}