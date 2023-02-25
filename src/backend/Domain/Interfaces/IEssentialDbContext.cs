using Microsoft.EntityFrameworkCore;
using WeeControl.Core.Domain.Contexts.Essentials;
using CustomerDbo = WeeControl.Core.Domain.Contexts.Essentials.CustomerDbo;

namespace WeeControl.Core.Domain.Interfaces;

public interface IEssentialDbContext : IDbContext
{
    DbSet<PersonDbo> Person { get; set; }
    DbSet<PersonIdentityDbo> PersonIdentities { get; set; }
    DbSet<AddressDbo> PersonAddresses { get; set; }

    DbSet<UserDbo> Users { get; set; }
    DbSet<UserNotificationDbo> UserNotifications { get; set; }
    DbSet<UserClaimDbo> UserClaims { get; set; }
    DbSet<UserSessionDbo> UserSessions { get; set; }
    DbSet<UserSessionLogDbo> SessionLogs { get; set; }
    
    DbSet<EmployeeDbo> Employees { get; set; }
    DbSet<CustomerDbo> Customers { get; set; }
    
    DbSet<UserFeedsDbo> Feeds { get; set; }
}
