using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.BoundedContexts.Credentials.DatabaseObjects;
using WeeControl.Backend.Domain.Interfaces;

namespace WeeControl.Backend.Domain.BoundedContexts.Credentials
{
    public interface ICredentialsDbContext : IDbContext
    {
        DbSet<UserDbo> Users { get; set; }

        DbSet<SessionDbo> Sessions { get; set; }

        DbSet<TerritoryDbo> Territories { get; set; }

        DbSet<ClaimDbo> Claims { get; set; }
    }
}
