using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Interfaces;
using WeeControl.Domain.Contexts.Essential;

namespace WeeControl.Persistence.Essential
{
    public class EssentialDbContext : DbContext, IEssentialDbContext
    {
        public DbSet<UserDbo> Users { get; set; }
        public DbSet<IdentityDbo> UserIdentities { get; set; }
        public DbSet<ClaimDbo> UserClaims { get; set; }
        public DbSet<NotificationDbo> UserNotifications { get; set; }

        public DbSet<SessionDbo> UserSessions { get; set; }
        public DbSet<SessionLogDbo> SessionLogs { get; set; }
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
