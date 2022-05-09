using System.Linq;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.Databases.Databases;
using WeeControl.Backend.Domain.Databases.Databases.DatabaseObjects.EssentialsObjects;
using WeeControl.Backend.Persistence.BoundedContext.Credentials.Configurations;
using WeeControl.Common.UserSecurityLib.BoundedContexts.HumanResources;

namespace WeeControl.Backend.Persistence.BoundedContext.Credentials
{
    public class EssentialDbContext : DbContext, IEssentialDbContext
    {
        public DbSet<UserDbo> Users { get; set; }

        public DbSet<SessionDbo> Sessions { get; set; }

        public DbSet<TerritoryDbo> Territories { get; set; }

        public DbSet<ClaimDbo> Claims { get; set; }

        public EssentialDbContext(DbContextOptions<EssentialDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            //Database.Migrate();
            Database.EnsureCreated();

            if (!Territories.Any())
            {
                var territory = new TerritoryDbo() { CountryCode = "EGP", TerritoryCode = "CAI", TerritoryName = "Cairo" };
                Territories.Add(territory);
                SaveChanges();

                var user = UserDbo.Create("admin@admin.com", "admin", "admin", territory.TerritoryCode);
                Users.Add(user);
                SaveChanges();

                var claim = new ClaimDbo() { UserId = user.UserId, ClaimType = HumanResourcesData.Role, ClaimValue = HumanResourcesData.Claims.Tags.SuperUser };
                Claims.Add(claim);
                SaveChanges();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SessionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClaimEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TerritoryEntityTypeConfiguration());
        }
    }
}
