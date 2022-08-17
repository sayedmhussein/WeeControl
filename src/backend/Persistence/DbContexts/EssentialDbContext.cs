using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Contexts;
using WeeControl.Domain.Contexts.Essential;

namespace WeeControl.Persistence.DbContexts
{
    public class EssentialDbContext : DbContext, IEssentialDbContext
    {
        public DbSet<PersonDbo> Person { get; set; }
        public DbSet<UserDbo> Users { get; set; }
        public DbSet<UserIdentityDbo> UserIdentities { get; set; }
        public DbSet<UserClaimDbo> UserClaims { get; set; }
        public DbSet<EmployeeDbo> Employees { get; set; }
        public DbSet<CustomerDbo> Customers { get; set; }
        public DbSet<UserNotificationDbo> UserNotifications { get; set; }
        public DbSet<UserSessionDbo> UserSessions { get; set; }
        public DbSet<UserSessionLogDbo> SessionLogs { get; set; }
        public DbSet<TerritoryDbo> Territories { get; set; }
        

        public EssentialDbContext(DbContextOptions<EssentialDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbo).Assembly);
        }
    }
}
