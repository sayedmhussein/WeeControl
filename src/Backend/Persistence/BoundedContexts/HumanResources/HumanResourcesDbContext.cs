using System.Linq;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.Entities;
using WeeControl.Backend.Persistence.BoundedContexts.HumanResources.Configurations;
using Address = WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.ValueObjects.Address;

namespace WeeControl.Backend.Persistence.BoundedContexts.HumanResources
{
    public class HumanResourcesDbContext : DbContext, IHumanResourcesDbContext
    {
        public HumanResourcesDbContext(DbContextOptions<HumanResourcesDbContext> options) : base(options)
        {
            Database.EnsureCreated();
            SetBasicData();
        }

        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<Session> Sessions { get; set; }

        public DbSet<Territory> Territories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EmployeeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SessionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TerritoryEntityTypeConfiguration());
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(HumanResourcesDbContext).Assembly);
        }

        private void SetBasicData()
        {
            string territoryCode = "NYC-US";
                
            if (Territories.Any() == false)
            {
                var territory = Territory.Create(territoryCode,"Parent Territory", "USA", new Address(){ CityName = "New York"});
                 Territories.Add(territory);
                 SaveChanges();
            }
                
            if ( Employees.Any() == false)
            {
                var admin = Employee.Create(territoryCode, "Admin", "Admin", "admin", "admin");
                Employees.Add(admin);
                SaveChanges();
                
            }
        }
    }
}