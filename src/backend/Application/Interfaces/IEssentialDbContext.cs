using Microsoft.EntityFrameworkCore;
using WeeControl.Domain.Contexts.Essential;
using WeeControl.Domain.Interfaces;

namespace WeeControl.Application.Interfaces;

public interface IEssentialDbContext : IDbContext
{
    DbSet<UserDbo> Users { get; set; }
    DbSet<IdentityDbo> UserIdentities { get; set; }
    DbSet<NotificationDbo> UserNotifications { get; set; }
    DbSet<SessionDbo> UserSessions { get; set; }
    DbSet<SessionLogDbo> SessionLogs { get; set; }
    DbSet<TerritoryDbo> Territories { get; set; }
    DbSet<ClaimDbo> UserClaims { get; set; }
}