using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.Essential.Entities;
using WeeControl.Backend.Domain.Interfaces;

namespace WeeControl.Backend.Application.EssentialContext;

public interface IEssentialDbContext : IDbContext
{
    DbSet<UserDbo> Users { get; set; }

    DbSet<SessionDbo> Sessions { get; set; }
    
    DbSet<SessionLogDbo> Logs { get; set; }

    DbSet<TerritoryDbo> Territories { get; set; }

    DbSet<ClaimDbo> Claims { get; set; }
}