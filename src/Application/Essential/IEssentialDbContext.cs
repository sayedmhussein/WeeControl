using Microsoft.EntityFrameworkCore;
using WeeControl.Domain.Contexts.Essential;
using WeeControl.Domain.Interfaces;

namespace WeeControl.Application.Essential;

public interface IEssentialDbContext : IDbContext
{
    DbSet<UserDbo> Users { get; set; }
    
    DbSet<IdentityDbo> UserIdentities { get; set; }

    DbSet<SessionDbo> Sessions { get; set; }
    
    DbSet<SessionLogDbo> Logs { get; set; }

    DbSet<TerritoryDbo> Territories { get; set; }

    DbSet<ClaimDbo> Claims { get; set; }
}