using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Domain.WeeControl.ApiApp.Domain.Business;
using WeeControl.Core.Domain.WeeControl.ApiApp.Domain.User;

namespace WeeControl.Core.Application.Interfaces;

public interface IEssentialDbContext : IDbContext
{
    DbSet<PersonDbo> Person { get; set; }
    DbSet<UserDbo> Users { get; set; }
    DbSet<UserIdentityDbo> UserIdentities { get; set; }
    DbSet<UserNotificationDbo> UserNotifications { get; set; }
    DbSet<UserSessionDbo> UserSessions { get; set; }
    DbSet<UserSessionLogDbo> SessionLogs { get; set; }
    DbSet<TerritoryDbo> Territories { get; set; }
    DbSet<UserClaimDbo> UserClaims { get; set; }
    DbSet<EmployeeDbo> Employees { get; set; }
    DbSet<CustomerDbo> Customers { get; set; }
}