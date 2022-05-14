using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.Databases.Essential.DatabaseObjects.EssentialsObjects;
using WeeControl.Backend.Domain.Interfaces;

namespace WeeControl.Backend.Domain.Databases.Essential;

public interface IEssentialDbContext : IDbContext
{
    DbSet<UserDbo> Users { get; set; }

    DbSet<SessionDbo> Sessions { get; set; }
    
    DbSet<SessionLogDbo> Logs { get; set; }

    DbSet<TerritoryDbo> Territories { get; set; }

    DbSet<ClaimDbo> Claims { get; set; }
}