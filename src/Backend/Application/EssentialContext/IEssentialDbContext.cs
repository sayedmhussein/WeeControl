using Microsoft.EntityFrameworkCore;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Domain.Interfaces;

namespace WeeControl.Application.EssentialContext;

public interface IEssentialDbContext : IDbContext
{
    DbSet<UserDbo> Users { get; set; }

    DbSet<SessionDbo> Sessions { get; set; }
    
    DbSet<SessionLogDbo> Logs { get; set; }

    DbSet<TerritoryDbo> Territories { get; set; }

    DbSet<ClaimDbo> Claims { get; set; }
}