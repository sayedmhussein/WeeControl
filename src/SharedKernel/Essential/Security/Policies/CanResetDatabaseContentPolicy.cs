namespace WeeControl.SharedKernel.Essential.Security.Policies;

public class CanResetDatabaseContentPolicy : PolicyBuilderBase
{
    public const string Name = nameof(CanResetDatabaseContentPolicy);

    public CanResetDatabaseContentPolicy()
    {
        Builder.RequireClaim(ClaimsTagsList.Claims.Developer);
        //Builder.Requirements.Add();
    }
}