using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Persistence.Essential.Configurations;

namespace WeeControl.Persistence.Essential
{
    public class EssentialDbContext : DbContext, IEssentialDbContext
    {
        public DbSet<UserDbo> Users { get; set; }

        public DbSet<SessionDbo> Sessions { get; set; }
        
        public DbSet<SessionLogDbo> Logs { get; set; }

        public DbSet<TerritoryDbo> Territories { get; set; }

        public DbSet<ClaimDbo> Claims { get; set; }

        public EssentialDbContext(DbContextOptions<EssentialDbContext> options) : base(options)
        {
        }

        public async Task ResetDatabaseAsync(CancellationToken cancellationToken)
        {
            await Database.EnsureDeletedAsync(cancellationToken);
            await Database.EnsureCreatedAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SessionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SessionLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClaimEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TerritoryEntityTypeConfiguration());
        }
    }
}
