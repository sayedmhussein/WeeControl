using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.Entities;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.ValueObjects;
using WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.Entities;
using WeeControl.Backend.Persistence.BoundedContexts.HumanResources.Configurations;
using WeeControl.Common.UserSecurityLib;
using Address = WeeControl.Backend.Domain.BoundedContexts.HumanResources.TerritoryModule.ValueObjects.Address;
using Claims = WeeControl.Backend.Domain.BoundedContexts.HumanResources.EmployeeModule.ValueObjects.Claims;

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
            string territoryCode = "NYC-US" + new Random().NextDouble();
                
            if (Territories.Any()) return;
            
            var territory = Territory.Create(territoryCode,"Parent Territory", "USA", new Address(){ CityName = "New York"});
            Territories.Add(territory);
            SaveChanges();

            if (Employees.Any()) return;
            
            var admin = Employee.Create(territoryCode, "Admin", "Admin", "admin", "admin");
            Employees.Add(admin);
            SaveChanges();

            var employee = Employees.First();
            
            employee.Claims.Add(new Claims()
            {
                ClaimType = SecurityClaims.HumanResources.Role, 
                ClaimValue = SecurityClaims.HumanResources.Tags.SuperUser + ";test",
                GrantedBy = admin
            });

            SaveChanges();
        }
    }
}