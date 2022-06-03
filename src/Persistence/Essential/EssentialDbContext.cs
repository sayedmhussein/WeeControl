using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.Essential;
using WeeControl.Domain.Essential.Entities;

namespace WeeControl.Persistence.Essential
{
    public class EssentialDbContext : DbContext, IEssentialDbContext
    {
        public DbSet<UserDbo> Users { get; set; }
        
        public DbSet<IdentityDbo> UserIdentities { get; set; }

        public DbSet<SessionDbo> Sessions { get; set; }
        
        public DbSet<SessionLogDbo> Logs { get; set; }

        public DbSet<TerritoryDbo> Territories { get; set; }

        public DbSet<ClaimDbo> Claims { get; set; }

        public EssentialDbContext(DbContextOptions<EssentialDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            // modelBuilder.ApplyConfiguration(new SessionEntityTypeConfiguration());
            // modelBuilder.ApplyConfiguration(new SessionLogEntityTypeConfiguration());
            // modelBuilder.ApplyConfiguration(new ClaimEntityTypeConfiguration());
            // modelBuilder.ApplyConfiguration(new TerritoryEntityTypeConfiguration());
        }
    }
}
