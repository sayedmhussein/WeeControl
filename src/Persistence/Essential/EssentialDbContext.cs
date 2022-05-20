using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeeControl.Application.EssentialContext;
using WeeControl.Domain.Essential.Entities;
using WeeControl.Persistence.Essential.Configurations;
using WeeControl.SharedKernel.Essential.Security;
using WeeControl.SharedKernel.Services;

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
            // Database.EnsureCreated();
            // if (Territories.Any() == false)
            // {
            //     var territory = TerritoryDbo.Create("def", null, "def", "Default");
            //     Territories.Add(territory);
            //     SaveChanges();
            //     
            //     var admin = UserDbo.Create("admin@admin.com", "admin", new PasswordSecurity().Hash("admin"), territory.TerritoryId);
            //     Users.Add(admin);
            //     SaveChanges();
            //     
            //     Claims.Add(ClaimDbo.Create(admin.UserId, 
            //         ClaimsTagsList.Claims.Developer, ClaimsTagsList.Tags.SuperUser,
            //         admin.UserId));
            //     SaveChanges();
            // }
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
