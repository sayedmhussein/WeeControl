using Microsoft.EntityFrameworkCore;
using WeeControl.Server.Domain.Administration;
using WeeControl.Server.Domain.Administration.Entities;
using WeeControl.Server.Persistence.Administration.Configutations;

namespace WeeControl.Server.Persistence.Administration
{
    public class AdministrationDbContext : DbContext, IAdministrationDbContext
    {
        public DbSet<Territory> Territories { get; set; }

        public AdministrationDbContext(DbContextOptions<AdministrationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TerritoryEntityTypeConfiguration());
        }
    }
}