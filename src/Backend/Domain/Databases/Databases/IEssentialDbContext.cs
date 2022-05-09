using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.Databases.Databases.DatabaseObjects.EssentialsObjects;
using WeeControl.Backend.Domain.Interfaces;

namespace WeeControl.Backend.Domain.Databases.Databases
{
    public interface IEssentialDbContext : IDbContext
    {
        DbSet<UserDbo> Users { get; set; }

        DbSet<SessionDbo> Sessions { get; set; }

        DbSet<TerritoryDbo> Territories { get; set; }

        DbSet<ClaimDbo> Claims { get; set; }
    }
}
