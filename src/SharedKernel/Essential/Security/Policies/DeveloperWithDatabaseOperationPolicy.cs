namespace WeeControl.SharedKernel.Essential.Security.Policies;

public class DeveloperWithDatabaseOperationPolicy : PolicyBuilderBase
{
    public const string Name = nameof(DeveloperWithDatabaseOperationPolicy);

    public DeveloperWithDatabaseOperationPolicy()
    {
        Builder.RequireClaim(ClaimsTagsList.Claims.Developer);
        //Builder.Requirements.Add();
    }
}