using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Domain.Contexts.Essentials;

namespace WeeControl.Core.Domain.Interfaces;

public interface IEssentialDbContext : IDbContext
{
    DbSet<UserFeedsDbo> Feeds { get; }
    DbSet<PersonDbo> Person { get; }
    DbSet<PersonIdentityDbo> PersonIdentities { get; }
    DbSet<PersonContactDbo> PersonContacts { get; }
    DbSet<AddressDbo> PersonAddresses { get; }

    DbSet<UserNotificationDbo> UserNotifications { get; }
    DbSet<UserClaimDbo> UserClaims { get; }
    DbSet<UserSessionDbo> UserSessions { get; }
    DbSet<UserSessionLogDbo> SessionLogs { get; }

    DbSet<EmployeeDbo> Employees { get; }
    DbSet<CustomerDbo> Customers { get; }
}